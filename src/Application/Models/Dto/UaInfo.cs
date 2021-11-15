namespace YA.WebClient.Application.Models.Dto;

public class UaInfo
{
    public string Ua { get; set; }
    public UaBrowser Browser { get; set; }
    public UaEngine Engine { get; set; }
    public UaOs Os { get; set; }
    public UaDevice Device { get; set; }
    public UaCpu Cpu { get; set; }
}
