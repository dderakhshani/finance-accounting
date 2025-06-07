using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1606;&#1711;&#1575;&#1588;&#1578; &#1576;&#1740;&#1606; &#1587;&#1585;&#1601;&#1589;&#1604; &#1581;&#1587;&#1575;&#1576;&#1607;&#1575; &#1608; &#1711;&#1585;&#1608;&#1607; &#1591;&#1585;&#1601; &#1581;&#1587;&#1575;&#1576;&#1607;&#1575; 
    /// </summary>
    public partial class AccountHeadRelReferenceGroup : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//سرفصل حساب 
        /// </summary>
        public int AccountHeadId { get; set; } = default!;

        /// <summary>
//کد گروه مرجع
        /// </summary>
        public int ReferenceGroupId { get; set; } = default!;

        /// <summary>
//شماره مرجع
        /// </summary>
        public int ReferenceNo { get; set; } = default!;

        /// <summary>
//بدهکار است؟
        /// </summary>
        public bool IsDebit { get; set; } = default!;

        /// <summary>
//معتبر است؟
        /// </summary>
        public bool IsCredit { get; set; } = default!;

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
         

        public virtual AccountHead AccountHead { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual AccountReferencesGroups ReferenceGroup { get; set; } = default!;
    }
}
