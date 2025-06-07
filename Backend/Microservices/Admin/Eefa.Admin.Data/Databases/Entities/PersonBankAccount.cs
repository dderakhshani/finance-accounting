using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class PersonBankAccount : BaseEntity
    {

        public int PersonId { get; set; } = default!;


        public int? BankId { get; set; }


        public string? BankBranchName { get; set; }


        public int AccountTypeBaseId { get; set; } = default!;


        public string AccountNumber { get; set; }


        public string? Description { get; set; }


        public bool IsDefault { get; set; } = default!;



        public virtual BaseValue AccountTypeBase { get; set; } = default!;
        public virtual Bank Bank { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
    }
}
