using System.Collections.Generic;
using Eefa.Common.Data;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// گروه های کالا
    /// </summary>
    public partial class CommodityCategory : BaseEntity
    {

        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string LevelCode { get; set; } = default!;
        public string Code { get; set; } = default!;

        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; }
        /// <description>
        /// کدواحد اندازه گیری
        ///</description>

        public int MeasureId { get; set; } = default!;
        /// <description>
        /// ترتیب نمایش
        ///</description>

        public int OrderIndex { get; set; } = default!;
        /// <description>
        /// آیا فقط قابل خواندن است؟
        ///</description>

        public bool IsReadOnly { get; set; } = default!;

        /// <description>
        /// نقش صاحب سند
        ///</description>


        public virtual MeasureUnit Measure { get; set; } = default!;

        public bool? RequireParentProduct { get; set; }

        public virtual CommodityCategory? Parent { get; set; }

        public virtual ICollection<BomItem>
            BomItems
        { get; set; } = default!;

        public virtual ICollection<Bom>
        Boms
        { get; set; } = default!;
        public virtual ICollection<Commodity>
Commodities
        { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty>
CommodityCategoryProperties
        { get; set; } = default!;
        public virtual ICollection<CommodityCategory>
InverseParent
        { get; set; } = default!;
        public virtual ICollection<Warehouse>
Warehous
        { get; set; } = default!;
    }
}
