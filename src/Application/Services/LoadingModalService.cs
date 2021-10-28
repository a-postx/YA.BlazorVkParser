using System;
using YA.WebClient.Application.Events;

namespace YA.WebClient.Application.Services
{
    public class LoadingModalService
    {
        public event EventHandler<LoadingModalMessageChangedEventArgs> OnSetMessage;

        public event EventHandler OnShow;
        public event EventHandler OnHide;

        public void Show()
        {
            OnShow?.Invoke(this, null);
        }

        public void Show(string message)
        {
            Show();
            SetMessage(message);
        }

        public void Hide()
        {
            OnHide?.Invoke(this, null);
        }

        public void SetMessage(string message)
        {
            OnSetMessage?.Invoke(this, new LoadingModalMessageChangedEventArgs { Message = message });
        }

        public void ClearMessage()
        {
            OnSetMessage?.Invoke(this, new LoadingModalMessageChangedEventArgs { Message = string.Empty });
        }
    }
}
