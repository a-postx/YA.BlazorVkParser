namespace YA.WebClient.Extensions;

public static class EnumExtensions
{
    public static string GetUiName(this VkPeriodicParsingTaskRate value)
    {
        switch (value)
        {
            case VkPeriodicParsingTaskRate.Unknown: throw new ArgumentOutOfRangeException(nameof(value), value, null);
            case VkPeriodicParsingTaskRate.None: return "Нет";
            case VkPeriodicParsingTaskRate.HalfHour: return "30 минут";
            case VkPeriodicParsingTaskRate.Hour: return "1 час";
            case VkPeriodicParsingTaskRate.TwoHours: return "2 часа";
            case VkPeriodicParsingTaskRate.ThreeHours: return "3 часа";
            case VkPeriodicParsingTaskRate.SixHours: return "6 часов";
            case VkPeriodicParsingTaskRate.TwelveHours: return "12 часов";
            case VkPeriodicParsingTaskRate.TwentyFourHours: return "1 день";
            case VkPeriodicParsingTaskRate.FortyEightHours: return "2 дня";
            case VkPeriodicParsingTaskRate.Nightly: return "каждую ночь";
            default:
                return string.Empty;
        }
    }
}
