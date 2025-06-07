using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1576;&#1585;&#1711;&#1607; &#1607;&#1575;&#1740; &#1670;&#1705;
    /// </summary>
    public partial class ChequeSheets : BaseEntity
    {
        public ChequeSheets()
        {
            FinancialRequestDetails = new HashSet<FinancialRequestDetails>();
            FinancialRequestPartials = new HashSet<FinancialRequestPartial>();
        }

         

        /// <summary>
//شماره دسته چک 
        /// </summary>
        public int? PayChequeId { get; set; }

        /// <summary>
//شماره سریال چک
        /// </summary>
        public string SheetSeqNumber { get; set; } = default!;

        /// <summary>
//شماره چک
        /// </summary>
        public string SheetUniqueNumber { get; set; } = default!;

        /// <summary>
//شماره سری چک 
        /// </summary>
        public string SheetSeriNumber { get; set; } = default!;
        public decimal TotalCost { get; set; } = default!;
        public int? AccountReferenceId { get; set; }
        public int BankBranchId { get; set; } = default!;

        /// <summary>
//تاریخ صدور
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
//تاریخ سر رسید 
        /// </summary>
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
//صادر کننده چک 
        /// </summary>
        public int? IssuerEmployeeId { get; set; }
        public string? Description { get; set; }

        /// <summary>
//آیا فعال است 
        /// </summary>
        public bool? IsActive { get; set; } = default!;

        public bool IsUsed { get; set; } = default!;

        public int? ChequeDocumentState { get; set; }

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


        public string? BranchName { get; set; }
        public int? ReceiveChequeReferenceId { get; set; }
        public int? ReceiveChequeReferenceGroupId { get; set; }
        public int? OwnerChequeReferenceId { get; set; }
        public int? OwnerChequeReferenceGroupId { get; set; }
        public bool? ApproveReceivedChequeSheet { get; set; }
        public int ChequeTypeBaseId { get; set; } = default!;
        public int? BankId { get; set; }
        public string? AccountNumber { get; set; }
        public int? IssueReferenceBankId { get; set; }


        public virtual AccountReferences IssueReferenceBank { get; set; } = default!;
        public virtual AccountReferences AccountReference { get; set; } = default!;
        public virtual Banks Bank { get; set; } = default!;
        public virtual BaseValues ChequeTypeBase { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual AccountReferences OwnerChequeReference { get; set; } = default!;
        public virtual AccountReferencesGroups OwnerChequeReferenceGroup { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual PayCheque PayCheque { get; set; } = default!;
        public virtual AccountReferences ReceiveChequeReference { get; set; } = default!;
        public virtual AccountReferencesGroups ReceiveChequeReferenceGroup { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetails { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartials { get; set; } = default!;
        public virtual ICollection<FinancialRequestAttachments> FinancialRequestAttachments { get; set; } = default!;


    }
}
