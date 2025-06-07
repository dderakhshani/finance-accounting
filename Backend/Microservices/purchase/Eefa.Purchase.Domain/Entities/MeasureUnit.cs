using System.Collections.Generic;
using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// واحد های اندازه گیری
    /// </summary>
    public partial class MeasureUnit: DomainBaseEntity
    {
        public int? ParentId { get; set; }
    /// <description>
            /// عنوان
    ///</description>
    
        public string Title { get; set; } = default!;
    /// <description>
            /// نام اختصاصی
    ///</description>
    
        public string? UniqueName { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual MeasureUnit? Parent { get; set; }
                public virtual ICollection<Commodity>
    Commodities { get; set; } = default!;
                public virtual ICollection<CommodityCategory>
    CommodityCategories { get; set; } = default!;
                public virtual ICollection<CommodityCategoryProperty>
    CommodityCategoryProperties { get; set; } = default!;
                public virtual ICollection<MeasureUnit>
    InverseParent { get; set; } = default!;
                public virtual ICollection<MeasureUnitConversion>
    MeasureUnitConversionDestinationMeasureUnits { get; set; } = default!;
                public virtual ICollection<MeasureUnitConversion>
    MeasureUnitConversionSourceMeasureUnits { get; set; } = default!;
    }
}
