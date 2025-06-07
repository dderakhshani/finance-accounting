namespace Library.Models
{
    public class Pagination
    {
        private int _pageIndex;
        private int _pageSize;
        private string _orderByProperty;

        public Pagination Paginator()
        {
            return this;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value;
        }


        public int PageIndex
        {
            get => _pageIndex <= 0 ? 1 : _pageIndex;
            set => _pageIndex = value < 1 ? 1 : value;
        }



        public int Skip => (_pageIndex - 1) * _pageSize;

        public int Take => _pageSize;

        public string OrderByProperty
        {
            get => _orderByProperty ?? "Id";
            set => _orderByProperty = string.IsNullOrEmpty(value) ? "Id DESC" : value;
        }
    }
}