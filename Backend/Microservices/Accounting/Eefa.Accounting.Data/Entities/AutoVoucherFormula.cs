using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class AutoVoucherFormula : BaseEntity
    {
        public int VoucherTypeId { get; set; } = default!;
        public int SourceVoucherTypeId { get; set; } = default!;

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
        public string? Formula { get; set; }

        /// <summary>
        /// شرط
        /// </summary>
        public string? Conditions { get; set; }
        public string? GroupBy { get; set; }

        public virtual AccountHead AccountHead { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual CodeVoucherGroup VoucherType { get; set; } = default!;
        public virtual CodeVoucherGroup SourceVoucherType { get; set; } = default!;

    }
}
