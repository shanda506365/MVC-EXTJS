
namespace USO.Domain
{
    using System.ComponentModel;

    public class PageQuery
    {
        public PageQuery()
        {
            SortDirection = ListSortDirection.Ascending;
            PageSize = 50;
            Page = 1;
        }
    
        public ListSortDirection SortDirection { get; set; }
        public string SortMember { get; set; }
    
        //query.Skip((Page - 1) * PageSize);
        public int Page { get; set; }
        public int PageSize { get; set; }       	
     
        public string Keyword { get; set; }
        public bool FilterByKeyword { get { return !string.IsNullOrWhiteSpace(Keyword); } }            
    }
}
