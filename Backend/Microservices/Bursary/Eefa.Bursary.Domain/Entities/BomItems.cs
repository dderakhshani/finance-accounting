using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class BomItems : BaseEntity
    {
         
        public int BomId { get; set; } = default!;
        public int? SubCategoryId { get; set; }
        public int? CommodityId { get; set; }

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
         

        public virtual Boms Bom { get; set; } = default!;
        public virtual Commodities Commodity { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
