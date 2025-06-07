using Library.Attributes;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    [HasUniqueIndex]
    public partial class CodeAccountHeadGroup : BaseEntity
    {
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// شناسه
        /// </summary>
        [UniqueIndex]
        public string Code { get; set; } = default!;

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
