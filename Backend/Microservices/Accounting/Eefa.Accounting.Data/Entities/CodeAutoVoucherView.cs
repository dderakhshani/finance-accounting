using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class CodeAutoVoucherView : BaseEntity
    {
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// مشاهده نام
        /// </summary>
        public string ViewName { get; set; } = default!;

        /// <summary>
        /// مشاهده عنوان
        /// </summary>
        public string ViewCaption { get; set; } = default!;


        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
