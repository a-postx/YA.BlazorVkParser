using Blazorise;

namespace YA.WebClient.Application.States
{
    public class ThemeOptionsState : IThemeOptionsState
    {
        public string ModalContentClass { get; set; }
        public string ModalContentStyle { get; set; }
        public string TableClass { get; set; }
        public ThemeContrast Contrast { get; set; }
        public string UserToggleBackgroundColorStyle { get; set; }
    }
}
