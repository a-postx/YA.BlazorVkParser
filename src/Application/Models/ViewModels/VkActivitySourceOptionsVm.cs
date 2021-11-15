namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Настройки источников активностей.
/// </summary>
public class VkActivitySourceOptionsVm
{
    /// <summary>
    /// Активности в постах.
    /// </summary>
    public bool Posts { get; set; }

    /// <summary>
    /// Активности в дискуссиях
    /// </summary>
    public bool Discussions { get; set; }
}
