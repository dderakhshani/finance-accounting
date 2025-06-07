using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class DocumentHeadPurchaseOrders : BaseEntity
    {

        /// <summary>
//شماره 
        /// </summary>
         

        /// <summary>
//شماره سند 
        /// </summary>
        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
//شماره سفارش خرید 
        /// </summary>
        public int PurchaseOrderId { get; set; } = default!;

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
