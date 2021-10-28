using Blazorise;

namespace YA.WebClient.Application.Interfaces
{
    public interface IThemeOptionsState
    {
        string ModalContentClass { get; set; }
        string TableClass { get; set; }
        ThemeContrast Contrast { get; set; }
        string UserToggleBackgroundColorStyle { get; set; }
    }
}
