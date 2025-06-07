namespace Eefa.Inventory.Domain
{
    public class SpCalculateRemainQuantityRequestParam
    {
        public string RequestNo { get; set; }
        public int CommodityId { get; set; }
       
       
        
    }
    public class CalculateRemainQuantityRequestCommodityByPersontParam
    {
        public string RequestNo { get; set; }
        public int RequestDocumentItemId { get; set; }
        public int CommodityId { get; set; }
        

    }
}
