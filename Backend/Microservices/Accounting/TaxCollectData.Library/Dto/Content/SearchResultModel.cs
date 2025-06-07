namespace TaxCollectData.Library.Dto.Content
{
    public record SearchResultModel<T>
    {
        public SearchResultModel(List<T> result, PaginationModel pagination)
        {
            Result = result;
            Pagination = pagination;
        }

        public List<T> Result { get; }

        public PaginationModel Pagination { get; }
    }
}