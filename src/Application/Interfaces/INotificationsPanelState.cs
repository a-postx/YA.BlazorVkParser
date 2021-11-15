namespace YA.WebClient.Application.Interfaces;

public interface INotificationsPanelState
{
    bool Visible { get; }

    event EventHandler<NotificationsPanelVisibilityChangedEventArgs> VisibilityUpdated;

    void Dispose();
    void UpdateVisibility(bool visible);
}
