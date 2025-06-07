using System.Collections.Generic;

namespace FileTransfer.WebApi.Services.Models
{
    public class ArchiveModel
    {
        public int Id { get; set; }
        public int BaseValueTypeId { get; set; } = default!;
        public string BaseValueTypeTitle { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string TypeBaseTitle { get; set; } = default!;
        public string FileNumber { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string KeyWords { get; set; } = default!;

        public List<AttachmentModel> Attachments { get; set; }
    }
}
