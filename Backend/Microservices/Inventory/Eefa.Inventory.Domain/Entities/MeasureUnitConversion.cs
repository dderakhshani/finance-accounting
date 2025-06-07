using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// تبدیل واحد های اندازه گیری
    /// </summary>
    public partial class MeasureUnitConversion: DomainBaseEntity
    {
        public int SourceMeasureUnitId { get; set; } = default!;
    /// <description>
            /// واحد اندازه گیری ثانویه
    ///</description>
    
        public int DestinationMeasureUnitId { get; set; } = default!;
    /// <description>
            /// ضریب تبدیل
    ///</description>
    
        public double? Ratio { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual MeasureUnit DestinationMeasureUnit { get; set; } = default!;
    public virtual MeasureUnit SourceMeasureUnit { get; set; } = default!;
    }
}
