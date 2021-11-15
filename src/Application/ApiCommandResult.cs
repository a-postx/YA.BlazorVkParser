namespace YA.WebClient.Application;

public class ApiCommandResult<TResult> : IApiCommandResult<TResult>
{
    private ApiCommandResult() { }

    public ApiCommandResult(ApiCommandStatus status, TResult data, string errorText, Guid requestId)
    {
        Status = status;
        Data = data;
        ErrorText = errorText;
        RequestId = requestId;
    }

    public ApiCommandStatus Status { get; protected set; }
    public TResult Data { get; protected set; }
    public string ErrorText { get; protected set; }
    public Guid RequestId { get; protected set; }
}
