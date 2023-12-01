using FoodDelivery.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Utilities
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; private set; }
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public PaginatedList(List<T> items, int totalItems, int currentPage, int pageSize)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            Items = items.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
