using Eefa.Common;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class CommodityCategoryProperty : BaseEntity, IHierarchical
    {

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد گروه
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// کد واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        /// <summary>
        /// قوانین حاکم بر مولفه
        /// </summary>
        public int? PropertyTypeBaseId { get; set; }

       

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;


        public virtual CommodityCategory? Category { get; set; } = default!;
        public virtual MeasureUnit? Measure { get; set; } = default!;
        public virtual CommodityCategoryProperty? Parent { get; set; } = default!;
        public virtual BaseValue? PropertyTypeBase { get; set; } = default!;
        public virtual ICollection<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValue> CommodityPropertyValues { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty> InverseParent { get; set; } = default!;
    }
}
