using System.Collections.Generic;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    public partial class CommodityCategoryProperty : DomainBaseEntity
    {
        public int? ParentId { get; set; }
        /// <description>
        /// کد گروه
        ///</description>

        public int? CategoryId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string LevelCode { get; set; } = default!;
        /// <description>
        /// نام اختصاصی
        ///</description>

        public string UniqueName { get; set; } = default!;
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// کد واحد اندازه گیری
        ///</description>

        public int? MeasureId { get; set; }
        /// <description>
        /// قوانین حاکم بر مولفه
        ///</description>

        public int? PropertyTypeBaseId { get; set; }
        /// <description>
        /// ترتیب نمایش
        ///</description>

        public int OrderIndex { get; set; } = default!;
        /// <description>
        /// نقش صاحب سند
        ///</description>


        public virtual CommodityCategory? Category { get; set; }
        public virtual MeasureUnit? Measure { get; set; }
        public virtual CommodityCategoryProperty? Parent { get; set; }
        public virtual BaseValue? PropertyTypeBase { get; set; }
        public virtual ICollection<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty> InverseParent { get; set; } = default!;
        public virtual ICollection<WarehouseLayoutProperty> WarehouseLayoutProperties { get; set; } = default!;
    }
}
