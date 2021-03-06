namespace YA.WebClient.Application.Interfaces;

public interface IModalUserWarningState
{
    ApiCommandStatus Status { get; }
    string ErrorMessage { get; }
    Guid? RequestId { get; }

    event EventHandler PropertiesUpdated;

    void Update(ApiCommandStatus status, string errorText, Guid? requestId);
    void Clear();
}
