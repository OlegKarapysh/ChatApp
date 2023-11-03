namespace Chat.WebUI.Services.Notifications;

public class NotificationService : INotificationService
{
    public event Action<string>? NotificationRaised;

    public void ShowError(string message)
    {
        NotificationRaised?.Invoke(message);
    }
}