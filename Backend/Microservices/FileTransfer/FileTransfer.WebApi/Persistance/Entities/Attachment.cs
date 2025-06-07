using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;

namespace FileTransfer.WebApi.Persistance.Entities
{
    [Table(name: "Attachment", Schema = "admin")]
    public partial class Attachment : BaseEntity
    {
        public Attachment()
        {
            
        }

        /// <summary>
        /// کد
        /// </summary>


        /// <summary>
        /// کد زبان
        /// </summary>
        public int LanguageId { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string Extention { get; set; } = default!;
        public bool IsUsed { get; set; }
        public string FileNumber { get; set; } = default!;


        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// کلمات کلیدی
        /// </summary>
        public string? KeyWords { get; set; }

        /// <summary>
        /// لینک
        /// </summary>
        public string Url { get; set; } = default!;


        public virtual User CreatedBy { get; set; } = default!;
        public virtual Language Language { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValue TypeBase { get; set; } = default!;
        public virtual List<ArchiveAttachments> ArchiveAttachments { get; set; } = default!;
    }
}
