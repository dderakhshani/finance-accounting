using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class CodeRowDescription : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;


        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
