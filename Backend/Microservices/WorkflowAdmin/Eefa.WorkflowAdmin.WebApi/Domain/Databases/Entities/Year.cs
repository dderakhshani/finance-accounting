using System;
using System.Collections.Generic;
using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    public partial class Year : BaseEntity
    {
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// نام سال
        /// </summary>
        public int YearName { get; set; } = default!;
        public bool CreateWithOutStartVoucher { get; set; } = default!;

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime FirstDate { get; set; } = default!;

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime LastDate { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool? IsEditable { get; set; } = default!;

        /// <summary>
        /// قابل شمارش است؟
        /// </summary>
        public bool IsCalculable { get; set; } = default!;

        /// <summary>
        /// آیا تاریخ در سال جاری است؟
        /// </summary>
        public bool IsCurrentYear { get; set; } = default!;

        /// <summary>
        /// تاریخ قفل شدن اطلاعات
        /// </summary>
        public DateTime? LastEditableDate { get; set; }


        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<UserYear> UserYears { get; set; } = default!;
    }
}
