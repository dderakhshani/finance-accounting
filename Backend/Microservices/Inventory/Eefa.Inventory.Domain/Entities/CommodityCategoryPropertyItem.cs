using System.Collections.Generic;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// آیتم های ویژگی گروه کالا
    /// </summary>
    public partial class CommodityCategoryPropertyItem : DomainBaseEntity
    {
        /// <description>
        /// کد ویژگی گروه
        ///</description>

        public int CategoryPropertyId { get; set; } = default!;
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// نام اختصاصی
        ///</description>

        public string UniqueName { get; set; } = default!;
        /// <description>
        /// کد
        ///</description>

        public string? Code { get; set; }
        /// <description>
        /// ترتیب نمایش
        ///</description>

        public int OrderIndex { get; set; } = default!;
        /// <description>
        /// فعال است؟
        ///</description>

        public bool IsActive { get; set; } = default!;


        public virtual CommodityCategoryProperty CategoryProperty { get; set; } = default!;
        public virtual CommodityCategoryPropertyItem? Parent { get; set; }
        public virtual ICollection<CategoryPropertyMapping>CategoryPropertyMappingCommodityCategoryPropertyItems1Navigation{ get; set; } = default!;
        public virtual ICollection<CategoryPropertyMapping>CategoryPropertyMappingCommodityCategoryPropertyItems2Navigation{ get; set; } = default!;
        public virtual ICollection<CommodityCategoryPropertyItem>InverseParent{ get; set; } = default!;
        public virtual ICollection<WarehouseLayoutProperty>WarehouseLayoutProperties{ get; set; } = default!;
    }
}
