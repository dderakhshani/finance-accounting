using System;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class Signer : BaseEntity
    {
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// عنوان امضاء کننده 
        /// </summary>
        public string SignerDescription { get; set; } = default!;

        /// <summary>
        /// چندمین امضاء کننده
        /// </summary>
        public int SignerOrderIndex { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool? IsActive { get; set; } = default!;

        /// <summary>
        /// تاریخ فعال شدن
        /// </summary>
        public DateTime ActiveDate { get; set; } = default!;

        /// <summary>
        /// تاریخ غیر فعال شدن
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
    }
}
