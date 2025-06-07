using Eefa.Common.Data;

namespace Eefa.Commodity.Data.Entities
{
    public partial class CommodityPropertyValue : BaseEntity
    {

        /// <summary>
        /// کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;

        /// <summary>
        /// کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;

        /// <summary>
        /// کد آیتم ویژگی مقدار 
        /// </summary>
        public int? ValuePropertyItemId { get; set; }

        /// <summary>
        /// مقدار
        /// </summary>
        public string? Value { get; set; }


        public virtual CommodityCategoryProperty CategoryProperty { get; set; } = default!;
        public virtual Commodity Commodity { get; set; } = default!;
        public virtual CommodityCategoryPropertyItem? ValuePropertyItem { get; set; } = default!;
    }
}
