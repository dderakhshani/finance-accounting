namespace TaxCollectData.Library.Dto.Content
{
    public record GetTokenDto
    {
        public GetTokenDto(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}