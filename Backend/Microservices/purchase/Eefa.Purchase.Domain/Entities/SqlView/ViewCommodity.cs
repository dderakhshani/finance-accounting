namespace Eefa.Purchase.Domain
{

    public partial class ViewCommodity
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
        public bool? IsHaveWast { get; set; } = default!;
        public bool? IsAsset { get; set; } = default!;


    }
    


}
