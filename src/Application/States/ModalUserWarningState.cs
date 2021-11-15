namespace YA.WebClient.Application.States;

public class ModalUserWarningState : IModalUserWarningState
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
