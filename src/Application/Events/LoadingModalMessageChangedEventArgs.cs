using System;

namespace YA.WebClient.Application.Events
{
    public class LoadingModalMessageChangedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
