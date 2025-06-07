using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1585;&#1578;&#1576;&#1575;&#1591; &#1576;&#1740;&#1606; &#1591;&#1585;&#1601; &#1581;&#1587;&#1575;&#1576;&#1607;&#1575; &#1608; &#1711;&#1585;&#1608;&#1607; &#1591;&#1585;&#1601; &#1581;&#1587;&#1575;&#1576;
    /// </summary>
    public partial class AccountReferencesRelReferencesGroups : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد طرف حساب
        /// </summary>
        public int ReferenceId { get; set; } = default!;

        /// <summary>
//کد گروه طرف حساب
        /// </summary>
        public int ReferenceGroupId { get; set; } = default!;

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
         

        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual AccountReferences Reference { get; set; } = default!;
        public virtual AccountReferencesGroups ReferenceGroup { get; set; } = default!;
    }
}
