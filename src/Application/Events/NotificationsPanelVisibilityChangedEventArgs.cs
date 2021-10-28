using System;

namespace YA.WebClient.Application.Events
{
    public class NotificationsPanelVisibilityChangedEventArgs : EventArgs
    {
        public bool Visible { get; set; }
    }
}
