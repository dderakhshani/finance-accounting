namespace FileTransfer.WebApi.Services.Models
{
    public class AttachmentModel
    {
        public AttachmentModel()
        {
            
        }
        public int Id { get; set; }
        public int LanguageId { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string Extention { get; set; } = default!;
        public bool IsUsed { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? KeyWords { get; set; }
        public string Url { get; set; } = default!;
        public double RelevanceScore { get; set; }
    }
}
