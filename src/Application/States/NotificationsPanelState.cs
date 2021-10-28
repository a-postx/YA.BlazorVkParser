using System;
using YA.WebClient.Application.Events;
using YA.WebClient.Application.Interfaces;

namespace YA.WebClient.Application.States
{
    public class NotificationsPanelState : INotificationsPanelState, IDisposable
    {
        private bool _visible;

        public bool Visible
        {
            get
            {
                return _visible;
            }
        }

        public event EventHandler<NotificationsPanelVisibilityChangedEventArgs> VisibilityUpdated;

        public void UpdateVisibility(bool visible)
        {
            _visible = visible;

            VisibilityUpdated?.Invoke(this, new NotificationsPanelVisibilityChangedEventArgs { Visible = _visible });
        }

        protected virtual void Dispose(bool disposing)
        {
            _visible = false;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
