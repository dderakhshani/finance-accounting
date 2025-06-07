namespace TaxCollectData.Library.Dto.Content
{
    public record KeyDto
    {
        public string Key { get; }

        public string Id { get; }

        public string Algorithm { get; }
        public int Purpose { get; }

        public KeyDto(string key, string id, string algorithm, int purpose)
        {
            Key = key;
            Id = id;
            Algorithm = algorithm;
            Purpose = purpose;
        }
    }
}