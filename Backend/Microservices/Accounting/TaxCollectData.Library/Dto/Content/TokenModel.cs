namespace TaxCollectData.Library.Dto.Content
{
    public record TokenModel
    {
        public TokenModel(string token, long expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }

        public string Token { get; }

        public long ExpiresIn { get; }
    }
}