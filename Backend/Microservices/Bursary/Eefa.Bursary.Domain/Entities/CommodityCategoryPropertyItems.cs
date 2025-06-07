using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1570;&#1740;&#1578;&#1605; &#1607;&#1575;&#1740; &#1608;&#1740;&#1688;&#1711;&#1740; &#1711;&#1585;&#1608;&#1607; &#1705;&#1575;&#1604;&#1575;
    /// </summary>
    public partial class CommodityCategoryPropertyItems : BaseEntity
    {
        public CommodityCategoryPropertyItems()
        {
            CategoryPropertyMappingsCommodityCategoryPropertyItems1Navigation = new HashSet<CategoryPropertyMappings>();
            CategoryPropertyMappingsCommodityCategoryPropertyItems2Navigation = new HashSet<CategoryPropertyMappings>();
            CommodityPropertyValues = new HashSet<CommodityPropertyValues>();
            InverseParent = new HashSet<CommodityCategoryPropertyItems>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
//کد
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
//ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
//فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        public virtual CommodityCategoryProperties CategoryProperty { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual CommodityCategoryPropertyItems Parent { get; set; } = default!;
        public virtual ICollection<CategoryPropertyMappings> CategoryPropertyMappingsCommodityCategoryPropertyItems1Navigation { get; set; } = default!;
        public virtual ICollection<CategoryPropertyMappings> CategoryPropertyMappingsCommodityCategoryPropertyItems2Navigation { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValues> CommodityPropertyValues { get; set; } = default!;
        public virtual ICollection<CommodityCategoryPropertyItems> InverseParent { get; set; } = default!;
    }
}
