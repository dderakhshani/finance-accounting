using System.Collections.Generic;
using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    public partial class AccountHead : BaseEntity
    {
        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;
        public int CodeLevel { get; set; } = default!;

        /// <summary>
        /// شناسه
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// طول کد
        /// </summary>
        public int? CodeLength { get; set; }
        public bool? LastLevel { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        /// <summary>
        /// نوع موازنه
        /// </summary>
        public int? BalanceId { get; set; }

        /// <summary>
        /// کنترل ماهیت حساب 
        /// </summary>
        public int BalanceBaseId { get; set; } = default!;

        /// <summary>
        /// شرح موازنه
        /// </summary>
        public string? BalanceName { get; set; }

        /// <summary>
        /// وضعیت سند
        /// </summary>
        public int TransferId { get; set; } = default!;

        /// <summary>
        /// شرح وضعیت سند
        /// </summary>
        public string? TransferName { get; set; }

        /// <summary>
        /// کد گروه
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// نوع ارز 
        /// </summary>
        public int CurrencyBaseTypeId { get; set; } = default!;

        /// <summary>
        /// ویژگی ارزی دارد ؟
        /// </summary>
        public bool CurrencyFlag { get; set; } = default!;

        /// <summary>
        /// تسعیر پذیر است ؟
        /// </summary>
        public bool ExchengeFlag { get; set; } = default!;

        /// <summary>
        /// ویژگی پیگیری دارد ؟ 
        /// </summary>
        public bool TraceFlag { get; set; } = default!;

        /// <summary>
        /// ویژگی تعداد دارد ؟
        /// </summary>
        public bool QuantityFlag { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool? IsActive { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual AccountHead? Parent { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
        public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
        public virtual ICollection<AccountHead> InverseParent { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
    }
}
