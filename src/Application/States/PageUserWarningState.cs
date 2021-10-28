using System;
using YA.WebClient.Application.Enums;
using YA.WebClient.Application.Interfaces;

namespace YA.WebClient.Application.States
{
    /// <summary>
    /// Кеш состояния предупреждений на страницах. Действует как промежуточное звено
    /// для возможности переопределить реакцию на неудачный запрос на конкретной странице.
    /// </summary>
    public class PageUserWarningState : IPageUserWarningState
    {
        private ApiCommandStatus _status;
        private string _errorText;
        private Guid? _requestId;

        public ApiCommandStatus Status
        {
            get
            {
                return _status;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorText;
            }
        }

        public Guid? RequestId
        {
            get
            {
                return _requestId;
            }
        }

        public event EventHandler PropertiesUpdated;

        public void Update(ApiCommandStatus status, string errorText, Guid? requestId)
        {
            _status = status;
            _errorText = errorText;
            _requestId = requestId;

            PropertiesUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void Clear()
        {
            _status = ApiCommandStatus.Unknown;
            _requestId = null;
            _errorText = null;

            PropertiesUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
