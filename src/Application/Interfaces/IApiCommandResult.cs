namespace YA.WebClient.Application.Interfaces;

public interface IApiCommandResult<TResult>
{
    public ApiCommandStatus Status { get; }
    public TResult Data { get; }
    public string ErrorText { get; }
    public Guid RequestId { get; }
}
