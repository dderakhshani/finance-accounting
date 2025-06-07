namespace TaxCollectData.Library.Dto.Content
{
    public record SearchDto
    {
        public SearchDto(int page, int size)
        {
            Page = page;
            Size = size;
        }

        public SearchDto(List<FilterDto> filters, int page, int size, List<OrderByDto> orderBy, bool skipCount)
        {
            Filters = filters;
            Page = page;
            Size = size;
            OrderBy = orderBy;
            SkipCount = skipCount;
        }

        public List<FilterDto> Filters { get; } = new();
        public int Page { get; } = 1;

        public int Size { get; } = 10;

        public bool SkipCount { get; }

        public List<OrderByDto> OrderBy { get; }

        public void AddFilter(FilterDto filterDto)
        {
            Filters.Add(filterDto);
        }
    }
}