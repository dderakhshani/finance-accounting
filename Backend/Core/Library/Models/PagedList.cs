namespace Library.Models
{
    public class PagedList : IPagedList
    {
        public object Data { get; set; }
        public int TotalCount { get; set; }
    }

    public interface IPagedList 
    {
        public object Data { get; set; }
        public int TotalCount { get; set; }
    }
}