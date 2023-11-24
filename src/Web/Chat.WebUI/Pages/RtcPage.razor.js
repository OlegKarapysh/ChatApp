export function setLocalStream(stream) {
    const localVideo = document.getElementById('localVideo');
    localVideo.srcObject = stream;
}
export function setRemoteStream(stream) {
    const remoteVideo = document.getElementById('remoteVideo');
    remoteVideo.srcObject = stream;
}