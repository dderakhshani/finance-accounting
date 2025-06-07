namespace TaxCollectData.Library.Dto.Content
{
    public record ServiceStuffModel
    {
        public ServiceStuffModel(decimal tax, string itemId)
        {
            Tax = tax;
            ItemId = itemId;
        }

        public decimal Tax { get; }
        public string ItemId { get; }
    }
}