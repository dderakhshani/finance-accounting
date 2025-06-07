using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Documents : BaseEntity
    {
        public Documents()
        {
            DocumentAttachments = new HashSet<DocumentAttachments>();
        }

         
        public int DocumentId { get; set; } = default!;
        public int DocumentNo { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;
        public int? ReferenceId { get; set; }
        public int DocumentTypeBaseId { get; set; } = default!;

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        public virtual ICollection<DocumentAttachments> DocumentAttachments { get; set; } = default!;
    }
}
