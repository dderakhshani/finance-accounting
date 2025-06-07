using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1578;&#1576;&#1583;&#1740;&#1604; &#1608;&#1575;&#1581;&#1583; &#1607;&#1575;&#1740; &#1575;&#1606;&#1583;&#1575;&#1586;&#1607; &#1711;&#1740;&#1585;&#1740;
    /// </summary>
    public partial class MeasureUnitConversions : BaseEntity
    {
        public MeasureUnitConversions()
        {
            DocumentItems = new HashSet<DocumentItems>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//واحد اندازه گیری اولیه
        /// </summary>
        public int SourceMeasureUnitId { get; set; } = default!;

        /// <summary>
//واحد اندازه گیری ثانویه
        /// </summary>
        public int DestinationMeasureUnitId { get; set; } = default!;

        /// <summary>
//ضریب تبدیل
        /// </summary>
        public double? Ratio { get; set; }

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
        public virtual MeasureUnits DestinationMeasureUnit { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual MeasureUnits SourceMeasureUnit { get; set; } = default!;
        public virtual ICollection<DocumentItems> DocumentItems { get; set; } = default!;
    }
}
