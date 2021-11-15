namespace YA.WebClient.Application.Models.Diagnostic;

public class EnvironmentInfo
{
    public string FrameworkVersion { get; set; }
    public string HostEnvironment { get; set; }
    public string Version { get; set; }
    public string RuntimeIdentifier { get; set; }
    public string CoreClrBuild { get; set; }
    public string CoreClrHash { get; set; }
    public string CoreFxBuild { get; set; }
    public string CoreFxHash { get; set; }
}
