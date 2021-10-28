using System;
using System.Collections.Generic;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Страница постраничного вывода, визуальная модель.
    /// </summary>
    public class PageInfo
    {
        private const string NextLinkItem = "next";
        private const string PreviousLinkItem = "previous";
        private const string FirstLinkItem = "first";
        private const string LastLinkItem = "last";

        /// <summary>
        /// Получает или устанавливает число страниц.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Получает или устанавливает значение, показывающее есть ли следующая страница.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Получает или устанавливает значение, показывающее есть ли предыдущая страница.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Получает или устанавливает адрес следующей страницы.
        /// </summary>
        public Uri NextPageUrl { get; set; }

        /// <summary>
        /// Получает или устанавливает адрес предыдущей страницы.
        /// </summary>
        public Uri PreviousPageUrl { get; set; }

        /// <summary>
        /// Получает или устанавливает адрес первой страницы.
        /// </summary>
        public Uri FirstPageUrl { get; set; }

        /// <summary>
        /// Получает или устанавливает адрес последней страницы.
        /// </summary>
        public Uri LastPageUrl { get; set; }

        /// <summary>
        /// Получает или устанавливает значение заголовка Link, добавляющее адреса к следующей,
        /// предыдущей, первой и последней страницам.
        /// См. https://tools.ietf.org/html/rfc5988#page-6
        /// Существует стандартный список типов относительных ссылок, напр. следующий, предыущий, первый, последний.
        /// См. https://www.iana.org/assignments/link-relations/link-relations.xhtml
        /// </summary>
        /// <returns>Значение заголовка Link.</returns>
        public string ToLinkHttpHeaderValue()
        {
            List<string> values = new List<string>(4);

            if (HasNextPage && NextPageUrl != null)
            {
                values.Add(GetLinkValueItem(NextLinkItem, NextPageUrl));
            }

            if (HasPreviousPage && PreviousPageUrl != null)
            {
                values.Add(GetLinkValueItem(PreviousLinkItem, PreviousPageUrl));
            }

            if (FirstPageUrl != null)
            {
                values.Add(GetLinkValueItem(FirstLinkItem, FirstPageUrl));
            }

            if (LastPageUrl != null)
            {
                values.Add(GetLinkValueItem(LastLinkItem, LastPageUrl));
            }

            return string.Join(", ", values);
        }

        private string GetLinkValueItem(string rel, Uri url) => FormattableString.Invariant($"<{url}>; rel=\"{rel}\"");
    }
}
