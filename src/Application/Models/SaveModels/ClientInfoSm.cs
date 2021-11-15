namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Информация о клиенте, модель сохранения
/// </summary>
public class ClientInfoSm
{
    /// <summary>
    /// Версия клиента.
    /// </summary>
    public string ClientVersion { get; set; }
    /// <summary>
    /// Производитель браузера.
    /// </summary>
    public string Browser { get; set; }
    /// <summary>
    /// Версия браузера.
    /// </summary>
    public string BrowserVersion { get; set; }
    /// <summary>
    /// Операционная система.
    /// </summary>
    public string Os { get; set; }
    /// <summary>
    /// Версия операционной системы.
    /// </summary>
    public string OsVersion { get; set; }
    /// <summary>
    /// Модель устройства.
    /// </summary>
    public string DeviceModel { get; set; }
    /// <summary>
    /// Разрешение экрана.
    /// </summary>
    public string ScreenResolution { get; set; }
    /// <summary>
    /// Размер активной части экрана браузера.
    /// </summary>
    public string ViewportSize { get; set; }
    /// <summary>
    /// Название страны
    /// </summary>
    public string CountryName { get; set; }
    /// <summary>
    /// Название региона страны.
    /// </summary>
    public string RegionName { get; set; }
    /// <summary>
    /// Текущее время в миллисекундах в формате Юникс.
    /// </summary>
    public long Timestamp { get; set; }
}
