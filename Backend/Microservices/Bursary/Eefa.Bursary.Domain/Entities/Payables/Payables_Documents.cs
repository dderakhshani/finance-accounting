using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    public partial class Payables_Documents : BaseEntity
    {
        public Payables_Documents()
        {
            Payables_DocumentsAccounts = new HashSet<Payables_DocumentsAccounts>();
            Payables_DocumentsOperations = new HashSet<Payables_DocumentsOperations>();
            Payables_DocumentsPayOrders = new HashSet<Payables_DocumentsPayOrders>();
        }

        public int PayTypeId { get; set; }
        public int? ChequeBookSheetId { get; set; }
        public int? BankAccountId { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public int MonetarySystemId { get; set; }
        public int? CreditAccountHeadId { get; set; }
        public int? CreditReferenceId { get; set; }
        public int? CreditReferenceGroupId { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int SubjectId { get; set; }
        public int? ChequeTypeId { get; set; }
        public int? StatusId { get; set; }
        public long CurrencyRate { get; set; }
        public long CurrencyAmount { get; set; }
        public long Amount { get; set; }
        public string Descp { get; set; }
        public long DocumentNo { get; set; }
        public bool ShowDebit { get; set; } = false;
        public bool ShowCredit { get; set; } = false;
        public string? ReferenceNumber { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? DraftDate { get; set; }

        public virtual BankAccounts BankAccount { get; set; }
        public virtual Payables_ChequeBooksSheets ChequeBookSheet { get; set; }
        public virtual BaseValues CurrencyTypeBase { get; set; }
        public virtual BaseValues PayType { get; set; }
        public virtual BaseValues Subject { get; set; }
        public virtual ICollection<Payables_DocumentsAccounts> Payables_DocumentsAccounts { get; set; }
        public virtual ICollection<Payables_DocumentsOperations> Payables_DocumentsOperations { get; set; }
        public virtual ICollection<Payables_DocumentsPayOrders> Payables_DocumentsPayOrders { get; set; }

    }
}