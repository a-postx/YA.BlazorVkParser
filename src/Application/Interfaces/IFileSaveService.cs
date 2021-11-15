namespace YA.WebClient.Application.Interfaces;

public interface IFileSaveService
{
    Task SaveAsBinaryContentAsync(string fileName, byte[] binaryContent);
    Task SaveAsStringContentAsync(string fileName, string textContent);
}
