using Eefa.Common;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class CommodityCategory : BaseEntity, IHierarchical
    {

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;
        /// <summary>
        /// کد
        /// </summary>
        public string Code { get; set; } = default!;
        public int CodingMode { get; set; } = default!;


        /// <summary>
        /// عنوان
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// کدواحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; } 

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;
        public bool? RequireParentProduct { get; set; }


        public virtual MeasureUnit Measure { get; set; } = default!;
        public virtual CommodityCategory? Parent { get; set; } = default!;

        public virtual ICollection<Bom> Boms { get; set; } = default!;
        public virtual ICollection<Commodity> Commodities { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = default!;
        public virtual ICollection<CommodityCategory> InverseParent { get; set; } = default!;
    }
}
