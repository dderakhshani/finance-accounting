namespace TaxCollectData.Library.Dto.Content
{
    public record OrderByDto
    {
        public OrderByDto(string name, bool asc)
        {
            Name = name;
            Asc = asc;
        }

        public string Name { get; }

        public bool Asc { get; }
    }
}