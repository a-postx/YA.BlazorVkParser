using System.Globalization;

namespace YA.WebClient.Extensions;

public static class LongExtensions
{
    public static string GetUiExecutionTime(this long executionTime)
    {
        string time = TimeSpan.FromMilliseconds(executionTime).ToString(@"mm\:ss", CultureInfo.InvariantCulture);
        string timeRu = time.Replace(":", "мин", StringComparison.InvariantCultureIgnoreCase);
        return timeRu.Insert(timeRu.Length, "сек");
    }
}
