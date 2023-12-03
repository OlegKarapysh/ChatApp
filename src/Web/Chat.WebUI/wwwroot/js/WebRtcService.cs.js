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
            urls: ["turn:freestun.net:3479"]
        }
    ]
}

let dotNet;
let localStream;
let remoteStream;
let peerConnection;
let isOffering;
let isOffered;

export function initialize(dotNetRef) {
    dotNet = dotNetRef;
}
export async function startLocalStream() {
    localStream = await navigator.mediaDevices.getUserMedia(mediaStreamConstraints);
    return localStream;
}
export function getRemoteStream() {
    return remoteStream;
}
function createPeerConnection() {
    peerConnection = new RTCPeerConnection(servers);
    peerConnection.addEventListener("icecandidate", handleConnection);
    peerConnection.addEventListener("addstream", gotRemoteMediaStream);
    peerConnection.addStream(localStream);
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
    let description = JSON.parse(descriptionText);
    await peerConnection.setRemoteDescription(description);
    let answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    dotNet.invokeMethodAsync("SendAnswer", JSON.stringify(answer));
}
export async function processCandidate(candidateText) {
    let candidate = JSON.parse(candidateText);
    await peerConnection.addIceCandidate(candidate);
}
export function hangupAction() {

    dotNet = null;
    localStream = null;
    remoteStream = null;
    isOffering = false;
    isOffered = false;
    peerConnection.close();
    peerConnection = null;
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
