namespace YA.WebClient.Application.Events;

public class ApiCallCompletedEventArgs : EventArgs
{
    public ApiCommandStatus Status { get; set; }
    public string Error { get; set; }
    public Guid? RequestId { get; set; }
}
