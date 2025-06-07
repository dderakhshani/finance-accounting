using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class DocumentItemsMeasurValues : BaseEntity
    {
         

        /// <summary>
//شماره سند 
        /// </summary>
        public int DocumentItemId { get; set; } = default!;

        /// <summary>
//کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;

        /// <summary>
//کد واحد فرعی کالا 
        /// </summary>
        public int? MeasurId { get; set; }

        /// <summary>
//مقدار کالا با کد واحد فرعی 
        /// </summary>
        public double? Quantity { get; set; }

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
         
    }
}
