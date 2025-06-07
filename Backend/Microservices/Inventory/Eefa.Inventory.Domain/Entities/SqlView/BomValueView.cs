namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// مقادیر فرمول ساخت
    /// </summary>
    public partial class BomValueView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; } = default!;
        public int BomValueHeaderId { get; set; } = default!;
        public int BomWarehouseId { get; set; } = default!;
        
        /// <description>
        /// کد کالای مصرفی
        ///</description>

        public int UsedCommodityId { get; set; } = default!;
    
    /// <description>
            /// مقدار
    ///</description>
    
        public double Value { get; set; } = default!;

        public int MeasureId { get; set; } = default!;
        public int CommodityCategoryId { get; set; } = default!;
        public string Title { get; set; } = default!;


    }
}
