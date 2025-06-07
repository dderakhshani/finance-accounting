namespace TaxCollectData.Library.Dto
{
    public record HttpResponse<T>
    {
        public HttpResponse(T body, int status)
        {
            Body = body;
            Status = status;
        }

        public HttpResponse(int status)
        {
            Status = status;
        }

        public T Body { get; }
        public int Status { get; }
    }
}