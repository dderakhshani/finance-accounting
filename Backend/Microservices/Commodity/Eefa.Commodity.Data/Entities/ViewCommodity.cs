namespace Eefa.Commodity.Data
{

    public partial class CommoditeisView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public int? MeasureId { get; set; }
        public string Code { get; set; }
        public int? CommodityCategoryId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string CategoryLevelCode { get; set; } = default!;
        public string CategoryTitle { get; set; } = default!;
        public string MeasureTitle { get; set; } = default!;
        public string SearchTerm { get; set; } = default!;
        public bool? IsConsumable { get; set; } = default!;
       
        public bool? IsAsset { get; set; } = default!;

        public bool? IsHaveWast { get; set; } = default!;
        public bool? IsHaveForceWast { get; set; }

        public bool? IsActive { get; set; }

        public string CommodityNationalId { get; set; } = default!;
        public string CommodityNationalTitle { get; set; } = default!;
        public string LevelCode { get; set; } = default!;
        public string TadbirCode { get; set; }
        public string CompactCode { get; set; }
        public string Descriptions { get; set; }
        /// <summary>
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
        /// حداقل تعداد
        /// </summary>
        public double MinimumQuantity { get; set; } = default!;

        /// <summary>
        /// حداکثر تعداد
        /// </summary>
        public double? MaximumQuantity { get; set; }

        /// <summary>
        /// تعداد سفارش
        /// </summary>
        public double? OrderQuantity { get; set; }

        /// <summary>
        /// نوع محاسبه قیمت
        /// </summary>
        public int? PricingTypeBaseId { get; set; }

        public int? BomsCount { get; set; }

       

    }



}
