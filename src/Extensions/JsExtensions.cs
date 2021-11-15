namespace YA.WebClient.Extensions;

public static class JsExtensions
{
    public async static Task SaveAs(this IJSRuntime js, string filename, byte[] data)
    {
        await js.InvokeAsync<object>(
            "saveAsFile",
            filename,
            Convert.ToBase64String(data));
    }

    public static async Task Alert(this IJSRuntime js, string message)
        => await js.InvokeAsync<object>("alert", message);

    public static async Task<string> Prompt(this IJSRuntime js, string message)
        => await js.InvokeAsync<string>("prompt", message);

    public static async Task Confirm(this IJSRuntime js, string message)
        => await js.InvokeAsync<string>("confirm", message);

    public static async Task<T> JsonParse<T>(this IJSRuntime js, string str)
        => await js.InvokeAsync<T>("JSON.parse", str);

    public static async Task<string> JsonStringify(this IJSRuntime js, object obj)
        => await js.InvokeAsync<string>("JSON.stringify", obj);

    public static async Task Print(this IJSRuntime js)
        => await js.InvokeAsync<string>("print");

    public static async Task ConsoleLog(this IJSRuntime js, object obj)
        => await js.InvokeAsync<string>("console.log", obj);
}
