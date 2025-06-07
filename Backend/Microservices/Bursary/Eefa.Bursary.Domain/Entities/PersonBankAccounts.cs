using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class PersonBankAccounts : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شخص
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
//نام بانک
        /// </summary>
        public int? BankId { get; set; }

        /// <summary>
//نام و کد شعبه 
        /// </summary>
        public string? BankBranchName { get; set; }

        /// <summary>
//نوع حساب بانکی 
        /// </summary>
        public int AccountTypeBaseId { get; set; } = default!;

        /// <summary>
//شماره حساب
        /// </summary>
        public string AccountNumber { get; set; } = default!;

        /// <summary>
//توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
//تلفن پیش فرض 
        /// </summary>
        public bool IsDefault { get; set; } = default!;

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
         

        public virtual BaseValues AccountTypeBase { get; set; } = default!;
        public virtual Banks Bank { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Persons Person { get; set; } = default!;
    }
}
