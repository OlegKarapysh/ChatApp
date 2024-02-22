namespace Chat.Domain.DTOs.Calls;

public class WebRtcSignalDto
{
    public string Channel { get; set; } = string.Empty;
    public WebRtcSignalType SignalType { get; set; }
    public string PayloadJson { get; set; } = string.Empty;
}