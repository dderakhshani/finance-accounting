using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class CommodityCategoryPropertyItem : BaseEntity
    {

        /// <summary>
        /// کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// کد
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;


        public virtual CommodityCategoryProperty CategoryProperty { get; set; } = default!;
        public virtual CommodityCategoryPropertyItem? Parent { get; set; } = default!;
        public virtual ICollection<CategoryPropertyMapping> CategoryPropertyMappingCommodityCategoryPropertyItems1Navigation { get; set; } = default!;
        public virtual ICollection<CategoryPropertyMapping> CategoryPropertyMappingCommodityCategoryPropertyItems2Navigation { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValue> CommodityPropertyValues { get; set; } = default!;
        public virtual ICollection<CommodityCategoryPropertyItem> InverseParent { get; set; } = default!;
    }
}
