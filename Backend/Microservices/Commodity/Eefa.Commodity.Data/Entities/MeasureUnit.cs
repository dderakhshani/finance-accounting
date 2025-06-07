using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class MeasureUnit : BaseEntity
    {
        public int? ParentId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string? UniqueName { get; set; }


        public virtual MeasureUnit? Parent { get; set; } = default!;
        public virtual ICollection<Commodity> Commodities { get; set; } = default!;
        public virtual ICollection<CommodityCategory> CommodityCategories { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = default!;
        public virtual ICollection<MeasureUnit> InverseParent { get; set; } = default!;
        public virtual ICollection<MeasureUnitConversion> MeasureUnitConversionDestinationMeasureUnits { get; set; } = default!;
        public virtual ICollection<MeasureUnitConversion> MeasureUnitConversionSourceMeasureUnits { get; set; } = default!;
    }
}
