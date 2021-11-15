namespace YA.WebClient.Infrastructure.Services;

public class FileSaver : IFileSaveService
{
    public FileSaver(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
    }

    private readonly IJSRuntime _jsRuntime;

    private async Task SaveAs(string fileName, string contentType, byte[] content)
    {
        await _jsRuntime.InvokeVoidAsync("BlazorDownloadFile", fileName, contentType, content);
    }

    public async Task SaveAsBinaryContentAsync(string fileName, byte[] binaryContent)
    {
        await SaveAs(fileName, "application/octet-stream", binaryContent);
    }

    public async Task SaveAsStringContentAsync(string fileName, string textContent)
    {
        byte[] content = System.Text.Encoding.UTF8.GetBytes(textContent);
        await SaveAs(fileName, "text/plain", content);
    }
}
