namespace USO.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using USO.Domain;

    public class PagedListViewModel<TItem> where TItem : class
    {
        public PagedListViewModel(IEnumerable<TItem> items, int currentPage, int itemPerPage, int totalCount)
        {
           
            Items = items;// new List<TItem>(items);
            CurrentPage = currentPage;
            ItemPerPage = itemPerPage > 0 ? itemPerPage : 1;
            TotalCount = totalCount;
        }

        public int CurrentPage
        {
            get;
            private set;
        }

        public int ItemPerPage
        {
            get;
            private set;
        }

        public int TotalCount
        {
            get;
            private set;
        }

        public int PageCount
        {
            [DebuggerStepThrough]
            get
            {
                return PageCalculator.TotalPage(TotalCount, ItemPerPage);
            }
        }

        public IEnumerable<TItem> Items
        {
            get;
            private set;
        }
    }  
}