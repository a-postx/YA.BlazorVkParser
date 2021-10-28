using System.Collections.Generic;

namespace YA.WebClient.Application.Models.ViewModels
{
    /// <summary>
    /// Постраничный результат вывода элементов общего типа.
    /// </summary>
    /// <typeparam name="T">Тип выводимого элемента.</typeparam>
    public class PaginatedResultVm<T> where T : class
    {
        /// <summary>
        /// Общее количество элементов.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Модель страницы.
        /// </summary>
        public PageInfo PageInfo { get; set; } = new PageInfo();

        /// <summary>
        /// Список элементов.
        /// </summary>
        public ICollection<T> Items { get; set; } = new List<T>();
    }
}
