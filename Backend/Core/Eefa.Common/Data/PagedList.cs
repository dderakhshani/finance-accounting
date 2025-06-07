using System.Collections.Generic;

namespace Eefa.Common.Data
{
    public class PagedList<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
        public decimal? TotalSum { get; set; }
    }

    //public interface IPagedList 
    //{
    //    public object Data { get; set; }
    //    public int TotalCount { get; set; }
    //}
}