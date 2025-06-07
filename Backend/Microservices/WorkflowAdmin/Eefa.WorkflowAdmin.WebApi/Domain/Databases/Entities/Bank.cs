using System.Collections.Generic;
using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{

    public partial class Bank : BaseEntity
    {
        public Bank()
        {
            PersonBankAccounts = new HashSet<PersonBankAccount>();
        }





        public string Code { get; set; } = default!;


        public string Title { get; set; } = default!;


        public string? GlobalCode { get; set; }


        public int TypeBaseId { get; set; } = default!;


        public string? SwiftCode { get; set; }


        public string? ManagerFullName { get; set; }


        public string? Descriptions { get; set; }


        public string? TelephoneJson { get; set; }


      

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValue TypeBase { get; set; } = default!;
        public virtual ICollection<PersonBankAccount> PersonBankAccounts { get; set; } = default!;
    }
}
