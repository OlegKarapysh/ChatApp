namespace Chat.WebUI.Services.Notifications;

public interface INotificationService
{
    event Action<string>? NotificationRaised;
    void ShowError(string message);
}