using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{

    public partial class AutoVoucherFormula : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد نوع سند
        /// </summary>
        public int VoucherTypeId { get; set; } = default!;

        /// <summary>
        /// ترتیب آرتیکل سند حسابداری
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// وضعیت مانده حساب
        /// </summary>
        public byte DebitCreditStatus { get; set; } = default!;

        /// <summary>
        /// کد سطح
        /// </summary>
        public int AccountHeadId { get; set; } = default!;

        /// <summary>
        /// توضیحات سطر
        /// </summary>
        public string? RowDescription { get; set; }

        /// <summary>
        /// ستونهای مقصد
        /// </summary>
        public string? DestinationFields { get; set; }

        /// <summary>
        /// ستونهای مبداء
        /// </summary>
        public string? SourceFields { get; set; }

        /// <summary>
        /// شرط
        /// </summary>
        public string? ConditionPart { get; set; }

        /// <summary>
        /// گروه شده با
        /// </summary>
        public string? GroupBy { get; set; }

        /// <summary>
        /// ترتیب
        /// </summary>
        public string? OrderBy { get; set; }

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
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual CodeVoucherGroup VoucherType { get; set; } = default!;
    }
}
