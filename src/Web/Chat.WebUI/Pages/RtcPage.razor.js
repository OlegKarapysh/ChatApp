export function setLocalStream(stream) {
    if (!stream) {
        console.log('Cannot set localStream: it is ' + stream);
    }
    
    const localVideo = document.getElementById('localVideo');
    localVideo.srcObject = stream;
}
export function setRemoteStream(stream) {
    if (!stream) {
        console.log('Cannot set remoteStream: it is ' + stream);
    }
    
    const remoteVideo = document.getElementById('remoteVideo');
    remoteVideo.srcObject = stream;
}