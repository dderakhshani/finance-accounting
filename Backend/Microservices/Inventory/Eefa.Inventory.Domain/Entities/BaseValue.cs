using System.Collections.Generic;
using Eefa.Common.Data;


namespace Eefa.Inventory.Domain
{
    public partial class BaseValue : BaseEntity
    {

        public int BaseValueTypeId { get; set; } = default!;
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }
        /// <description>
        /// کد
        ///</description>

        public string Code { get; set; } = default!;
        /// <description>
        /// کد سطح
        ///</description>

        public string LevelCode { get; set; } = default!;
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// نام اختصاصی
        ///</description>

        public string UniqueName { get; set; } = default!;
        /// <description>
        /// مقدار
        ///</description>

        public string Value { get; set; } = default!;
        /// <description>
        /// ترتیب آرتیکل سند 
        ///</description>

        public int OrderIndex { get; set; } = default!;
        /// <description>
        /// آیا فقط قابل خواندن است؟
        ///</description>

        public bool IsReadOnly { get; set; } = default!;
        /// <description>
        /// نقش صاحب سند
        ///</description>


        public virtual ICollection<Commodity>Commodities{ get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty>CommodityCategoryProperties{ get; set; } = default!;
       
        public virtual ICollection<WarehouseLayout>WarehouseLayouts{ get; set; } = default!;
    }
}
