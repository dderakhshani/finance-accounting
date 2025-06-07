namespace TaxCollectData.Library.Dto.Content
{
    public record ServerInformationModel
    {
        public ServerInformationModel(long serverTime, List<KeyDto> publicKeys)
        {
            ServerTime = serverTime;
            PublicKeys = publicKeys;
        }

        public long ServerTime { get; }

        public List<KeyDto> PublicKeys { get; }
    }
}