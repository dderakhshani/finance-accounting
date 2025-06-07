using Library.Common;

namespace Eefa.Audit.Data.Databases.Entities
{
    public partial class UserCompany : BaseEntity
    {
        /// <summary>
        /// کد کاربر
        /// </summary>
        public int UserId { get; set; } = default!;

        /// <summary>
        /// کد شرکت 
        /// </summary>
        public int CompanyInformationsId { get; set; } = default!;


        public virtual CompanyInformation CompanyInformations { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}
