using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class BaseValue : BaseEntity
    {

        /// <summary>
        /// کد نوع مقدار
        /// </summary>
        public int BaseValueTypeId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// مقدار
        /// </summary>
        public string Value { get; set; } = default!;

        /// <summary>
        /// ترتیب آرتیکل سند 
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;


        public virtual ICollection<Commodity> Commodities { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = default!;
    }
}
