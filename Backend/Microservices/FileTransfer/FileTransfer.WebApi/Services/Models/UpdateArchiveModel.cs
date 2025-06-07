using System.Collections.Generic;

namespace FileTransfer.WebApi.Services.Models
{
    public class UpdateArchiveModel
    {
        public int Id { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string FileNumber { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string KeyWords { get; set; } = default!;

        public List<int> AttachmentIds { get; set; }
    }
}
