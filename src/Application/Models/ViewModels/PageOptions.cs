using System.ComponentModel.DataAnnotations;

namespace YA.WebClient.Application.Models.ViewModels;

/// <summary>
/// Настройки постраничного вывода.
/// </summary>
public class PageOptions
{
    [Range(1, 20)]
    public int? First { get; set; }

    [Range(1, 20)]
    public int? Last { get; set; }

    public string After { get; set; }

    public string Before { get; set; }
}
