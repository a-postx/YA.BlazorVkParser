using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using YA.Common.Extensions;

namespace YA.WebClient.Extensions;

public static class StringExtensions
{
    public static T GetFromQueryString<T>(this string queryString) where T : new()
    {
        NameValueCollection queryParams = HttpUtility.ParseQueryString(queryString);
        Dictionary<string, string> dict = queryParams.Cast<string>().ToDictionary(k => k, v => queryParams[v]);

        T obj = new T();
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            string valueAsString = dict.GetValue(property.Name);

            if (valueAsString == null)
                continue;

            property.SetValue(obj, valueAsString, null);
        }

        return obj;
    }
}
