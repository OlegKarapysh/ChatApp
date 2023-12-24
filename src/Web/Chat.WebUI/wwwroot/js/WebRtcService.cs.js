"use strict";
const mediaStreamConstraints = {
    video: true,
    audio: true
};
const offerOptions = {
    offerToReceiveVideo: 1,
    offerToReceiveAudio: 1
};
const servers = {
    iceServers: [
        {
            urls: ["stun:stun1.l.google.com:19302", "stun:stun2.l.google.com:19302"]
        }
    ]
}
let dotNet;
let localFullStream, localVideoStream, remoteStream;
let peerConnection;
let isOffering, isOffered;

export function initialize(dotNetRef) {
    dotNet = dotNetRef;
}
export async function startLocalStream() {
    while (!localFullStream) {
        localFullStream = await navigator.mediaDevices.getUserMedia(mediaStreamConstraints);
    }
    
    const videoTrack = localFullStream.getVideoTracks()[0];
    localVideoStream = new MediaStream();
    localVideoStream.addTrack(videoTrack);
    
    return localVideoStream;
}
export function getRemoteStream() {
    return remoteStream;
}
function createPeerConnection() {
    peerConnection = new RTCPeerConnection(servers);
    peerConnection.addEventListener("icecandidate", handleConnection);
    peerConnection.addEventListener("addstream", gotRemoteMediaStream);
    peerConnection.addStream(localFullStream);
}
export async function callAction() {
    if (isOffered) return Promise.resolve();

    isOffering = true;
    createPeerConnection();
    let offerDescription = await peerConnection.createOffer(offerOptions);
    await peerConnection.setLocalDescription(offerDescription);
    return JSON.stringify(offerDescription);
}
export async function processAnswer(descriptionText) {
    let description = JSON.parse(descriptionText);
    await peerConnection.setRemoteDescription(description);
}
export async function processOffer(descriptionText) {
    if (isOffering) return;
    createPeerConnection();
    const description = JSON.parse(descriptionText);
    await peerConnection.setRemoteDescription(description);
    const answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    dotNet.invokeMethodAsync("SendAnswer", JSON.stringify(answer));
}
export async function processCandidate(candidateText) {
    let candidate = JSON.parse(candidateText);
    await peerConnection.addIceCandidate(candidate);
}
export async function hangupAction() {

    await peerConnection.close();
    dotNet = null;
    localFullStream = null;
    localVideoStream = null;
    remoteStream = null;
    isOffering = false;
    isOffered = false;
}
async function gotRemoteMediaStream(event) {
    const mediaStream = event.stream;
    remoteStream = mediaStream;
    await dotNet.invokeMethodAsync("SetRemoteStream");
}
async function handleConnection(event) {
    const iceCandidate = event.candidate;
    if (iceCandidate) {
        await dotNet.invokeMethodAsync("SendCandidate", JSON.stringify(iceCandidate));
    }
}
