using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1583;&#1587;&#1578;&#1607; &#1670;&#1705;
    /// </summary>
    public partial class PayCheque : BaseEntity
    {
        public PayCheque()
        {
            ChequeSheets = new HashSet<ChequeSheets>();
        }



        /// <summary>
        //شماره حساب بانک 
        /// </summary>
        public int BankAccountId { get; set; } = default!;
        public int ChequeTypeBaseId { get; set; } = default!;

        /// <summary>
//تعداد برگ 
        /// </summary>
        public int SheetsCount { get; set; } = default!;

        /// <summary>
//شماره دسته چک 
        /// </summary>
        public string ChequeNumberIdentification { get; set; } = default!;

        /// <summary>
//کد صاحب چک 
        /// </summary>
        public int? OwnerEmployeeId { get; set; }

        /// <summary>
//تاریخ تحویل چک 
        /// </summary>
        public DateTime? SetOwnerTime { get; set; }

        /// <summary>
//اتمام دسته چک 
        /// </summary>
        public bool IsFinished { get; set; } = default!;

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
        public virtual BaseValues ChequeTypeBase { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheets { get; set; } = default!;
    }
}
