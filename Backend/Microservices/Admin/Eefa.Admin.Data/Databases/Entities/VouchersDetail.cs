using System;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class VouchersDetail : BaseEntity
    {
         

        /// <summary>
        /// کد سند
        /// </summary>
        public int VoucherId { get; set; } = default!;

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        /// <summary>
        /// کد حساب سرپرست
        /// </summary>
        public int AccountHeadId { get; set; } = default!;
        public int AccountReferencesGroupId { get; set; } = default!;

        /// <summary>
        /// شرح آرتیکل  سند
        /// </summary>
        public string VoucherRowDescription { get; set; } = default!;

        /// <summary>
        /// بدهکار
        /// </summary>
        public double Debit { get; set; } = default!;

        /// <summary>
        /// اعتبار
        /// </summary>
        public double Credit { get; set; } = default!;

        /// <summary>
        /// ترتیب سطر
        /// </summary>
        public int? RowIndex { get; set; }

        /// <summary>
        /// شماره سند مرتبط 
        /// </summary>
        public int? DocumentId { get; set; }

        /// <summary>
        /// شماره مرجع
        /// </summary>
        public int? ReferenceId { get; set; }

        /// <summary>
        /// تاریخ مرجع
        /// </summary>
        public DateTime? ReferenceDate { get; set; }

        /// <summary>
        /// مقدار مرجع
        /// </summary>
        public double? ReferenceQty { get; set; }

        /// <summary>
        /// کد مرجع1
        /// </summary>
        public int? ReferenceId1 { get; set; }

        /// <summary>
        /// کد مرجع2
        /// </summary>
        public int? ReferenceId2 { get; set; }

        /// <summary>
        /// کد مرجع3
        /// </summary>
        public int? ReferenceId3 { get; set; }

        /// <summary>
        /// سطح 1
        /// </summary>
        public int? Level1 { get; set; }

        /// <summary>
        /// سطح 2
        /// </summary>
        public int? Level2 { get; set; }

        /// <summary>
        /// سطح 3
        /// </summary>
        public int? Level3 { get; set; }

        /// <summary>
        /// وضعیت مانده حساب
        /// </summary>
        public byte? DebitCreditStatus { get; set; }

        /// <summary>
        /// باقیمانده
        /// </summary>
        public long? Remain { get; set; }

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
        

        public virtual AccountHead AccountHead { get; set; } = default!;
        public virtual AccountReferencesGroup AccountReferencesGroup { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual DocumentHead? Document { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual AccountReference? ReferenceId1Navigation { get; set; } = default!;
        public virtual AccountReference? ReferenceId2Navigation { get; set; } = default!;
        public virtual AccountReference? ReferenceId3Navigation { get; set; } = default!;
        public virtual VouchersHead Voucher { get; set; } = default!;
    }
}
