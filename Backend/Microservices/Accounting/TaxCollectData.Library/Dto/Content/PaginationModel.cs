namespace TaxCollectData.Library.Dto.Content
{
    public record PaginationModel
    {
        public int Size { get; }

        public int Page { get; }

        public long Total { get; }
        
        public PaginationModel(int size, int page, long total)
        {
            Page = page;
            Total = total;
            Size = size;
        }
    }
}