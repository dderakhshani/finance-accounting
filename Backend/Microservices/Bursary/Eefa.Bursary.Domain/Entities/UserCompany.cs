using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1588;&#1585;&#1705;&#1578; &#1607;&#1575;&#1740;&#1740; &#1705;&#1607; &#1705;&#1575;&#1585;&#1576;&#1585; &#1576;&#1607; &#1570;&#1606;&#1607;&#1575; &#1583;&#1587;&#1578;&#1585;&#1587;&#1740; &#1583;&#1575;&#1585;&#1583;
    /// </summary>
    public partial class UserCompany : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد کاربر
        /// </summary>
        public int UserId { get; set; } = default!;

        /// <summary>
//کد اطلاعات شرکت 
        /// </summary>
        public int CompanyInformationsId { get; set; } = default!;

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        public virtual CompanyInformations CompanyInformations { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Users User { get; set; } = default!;
    }
}
