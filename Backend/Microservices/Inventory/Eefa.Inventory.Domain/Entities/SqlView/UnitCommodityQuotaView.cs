namespace Eefa.Inventory.Domain
{

    public partial class UnitCommodityQuotaView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public int UnitId { get; set; } = default!;
        public int CommodityQuota { get; set; } = default!;
        public int QuotaDays { get; set; } = default!;

        public string UnitsTitle { get; set; } = default!;
        public string UnitsCode { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;
        public string MeasureTitle { get; set; } = default!;
        public string CategoryTitle { get; set; } = default!;
        public string SearchTerm { get; set; } = default!;
        public int QuotaGroupsId { get; set; } = default!;
        public string QuotaGroupName { get; set; } = default!;

    }
    


}
