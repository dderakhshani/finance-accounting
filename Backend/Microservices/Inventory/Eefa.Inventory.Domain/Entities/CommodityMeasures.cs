using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// تبدیل واحد های اندازه گیری
    /// </summary>
    public partial class CommodityMeasures : DomainBaseEntity
    {
        public int CommodityId { get; set; } = default!;
    
    
        public int MeasureId { get; set; } = default!;
    
    
        public int OrderIndex { get; set; }
    
    
   
    }
}
