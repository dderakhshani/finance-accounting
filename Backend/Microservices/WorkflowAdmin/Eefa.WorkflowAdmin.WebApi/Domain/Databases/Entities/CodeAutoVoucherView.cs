using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{

    public partial class CodeAutoVoucherView : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// مشاهده نام
        /// </summary>
        public string ViewName { get; set; } = default!;

        /// <summary>
        /// مشاهده عنوان
        /// </summary>
        public string ViewCaption { get; set; } = default!;

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
