using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class PersonFingerprint : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// شماره انگشت
        /// </summary>
        public int FingerBaseId { get; set; } = default!;

        /// <summary>
        /// الگوی اثر انگشت
        /// </summary>
        public string FingerPrintTemplate { get; set; } = default!;

        /// <summary>
        /// عکس اثر انگشت
        /// </summary>
        public string? FingerPrintPhotoURL { get; set; }

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
        public virtual BaseValue FingerBase { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
    }
}
