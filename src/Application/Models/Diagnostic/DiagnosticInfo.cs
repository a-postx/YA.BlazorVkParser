namespace YA.WebClient.Application.Models.Diagnostic
{
    public class DiagnosticInfo
    {
        public string ClientName { get; set; }
        public string ClientVersion { get; set; }
        public string CurrentUri { get; set; }
        public EnvironmentInfo Environment { get; set; }
        public SecurityInfo Security { get; set; }
    }
}
