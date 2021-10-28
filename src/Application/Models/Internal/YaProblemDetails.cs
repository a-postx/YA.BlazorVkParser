namespace YA.WebClient.Application.Models.Internal
{
    public class YaProblemDetails
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public string CorrelationId { get; set; }
        public string TraceId { get; set; }
    }
}
