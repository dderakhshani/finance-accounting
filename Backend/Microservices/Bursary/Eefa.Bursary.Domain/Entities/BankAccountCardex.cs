using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class BankAccountCardex : BaseEntity
    {
         
        public int BankAccountId { get; set; } = default!;

        /// <summary>
//حساب پشتیبان
        /// </summary>
        public int? Amount { get; set; }

 
        public int InputMode { get; set; } = default!;
         

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
         

        public virtual BankAccounts BankAccount { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
