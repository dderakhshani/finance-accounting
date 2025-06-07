using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    /// <summary>
   // &#1588;&#1593;&#1576; &#1576;&#1575;&#1606;&#1705;
    /// </summary>
    public partial class BankBranches : BaseEntity
    {
        public BankBranches()
        {
            BankAccounts = new HashSet<BankAccounts>();
            PersonBankAcounts = new HashSet<PersonBankAcounts>();
        }


        public int BankId { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int CountryDivisionId { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string PhoneNumber { get; set; }
        public string ManagerFullName { get; set; }

        public virtual Banks Bank { get; set; } = default!;
        public virtual CountryDivisions CountryDivision { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual ICollection<BankAccounts> BankAccounts { get; set; } = default!;
        public virtual ICollection<PersonBankAcounts> PersonBankAcounts { get; set; } = default!;
    }
}
