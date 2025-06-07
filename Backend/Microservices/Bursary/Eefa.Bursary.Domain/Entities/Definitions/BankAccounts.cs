using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    /// <summary>
   // &#1581;&#1587;&#1575;&#1576;&#1607;&#1575;&#1740; &#1576;&#1575;&#1606;&#1705;&#1740;
    /// </summary>
    public partial class BankAccounts : BaseEntity
    {
        public BankAccounts()
        {
            BankAccountCardexes = new HashSet<BankAccountCardex>();
            InverseParent = new HashSet<BankAccounts>();
            PayCheques = new HashSet<PayCheque>();
            Payables_ChequeBooks = new HashSet<Payables_ChequeBooks>();
            Payables_Documents = new HashSet<Payables_Documents>();
        }

        public int? ParentId { get; set; }
        public int BankBranchId { get; set; } = default!;
        public string Sheba { get; set; } = default!;
        public string AccountNumber { get; set; } = default!;
        public int? SubsidiaryCodeId { get; set; }
        public int? RelatedBankAccountId { get; set; }
        public int AccountTypeBaseId { get; set; } = default!;
        public int AccountHeadId { get; set; } = default!;
        public int? AccountReferencesGroupId { get; set; }
        public int? ReferenceId { get; set; }
        public int? AccountStatus { get; set; }
        public decimal WithdrawalLimit { get; set; } = default!;
        public bool HaveChekBook { get; set; } = default!;
        public int CurrenceTypeBaseId { get; set; } = default!;
        public string SignersJson { get; set; } = default!;

        public virtual BaseValues AccountTypeBase { get; set; } = default!;
        public virtual BankBranches BankBranch { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual BankAccounts Parent { get; set; } = default!;
        public virtual ICollection<BankAccountCardex> BankAccountCardexes { get; set; } = default!;
        public virtual ICollection<BankAccounts> InverseParent { get; set; } = default!;
        public virtual ICollection<PayCheque> PayCheques { get; set; } = default!;
        public virtual ICollection<Payables_ChequeBooks> Payables_ChequeBooks { get; set; }
        public virtual ICollection<Payables_Documents> Payables_Documents { get; set; }

    }
}
