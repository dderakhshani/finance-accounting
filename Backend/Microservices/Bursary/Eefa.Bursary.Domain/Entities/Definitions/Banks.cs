using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    /// <summary>
   // &#1576;&#1575;&#1606;&#1705; &#1607;&#1575;
    /// </summary>
    public partial class Banks : BaseEntity
    {
        public Banks()
        {
            BankBranches = new HashSet<BankBranches>();
            ChequeSheets = new HashSet<ChequeSheets>();
            PersonBankAccounts = new HashSet<PersonBankAccounts>();
            PersonBankAcounts = new HashSet<PersonBankAcounts>();
        }


        /// <summary>
        //شناسه
        /// </summary>


        /// <summary>
        //کد
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//کد شعبه در بانک مرکزی
        /// </summary>
        public string GlobalCode { get; set; }

        /// <summary>
//نوع بانک - موسسه مالی اعتباری، قرض الحسنه و ...
        /// </summary>
        public int TypeBaseId { get; set; } = default!;

        /// <summary>
//سوئیفت
        /// </summary>
        public string SwiftCode { get; set; }

        /// <summary>
//نام مدیر بانک
        /// </summary>
        public string ManagerFullName { get; set; }

        /// <summary>
//توضیحات
        /// </summary>
        public string Descriptions { get; set; }

        /// <summary>
//شماره تلفن
        /// </summary>
        public string TelephoneJson { get; set; }

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


        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual BaseValues TypeBase { get; set; } = default!;
        public virtual ICollection<BankBranches> BankBranches { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheets { get; set; } = default!;
        public virtual ICollection<PersonBankAccounts> PersonBankAccounts { get; set; } = default!;
        public virtual ICollection<PersonBankAcounts> PersonBankAcounts { get; set; } = default!;
    }
}
