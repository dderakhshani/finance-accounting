using Eefa.Common.Domain;

namespace Eefa.Commodity.Data.Entities
{
    public partial class BomValue : DomainBaseEntity
    {


        /// <summary>
        /// کد سند فرمول ساخت
        /// </summary>
        public int BomValueHeaderId { get; set; } = default!;

        /// <summary>
        /// کد کالای مصرفی
        /// </summary>
        public int UsedCommodityId { get; set; } = default!;


        /// <summary>
        /// مقدار
        /// </summary>
        public double Value { get; set; } = default!;
        public int BomWarehouseId { get; set; } = default!;

        public virtual BomValueHeader BomValueHeader { get; set; } = default!;
       
        public virtual Commodity UsedCommodity { get; set; } = default!;
    }
}
