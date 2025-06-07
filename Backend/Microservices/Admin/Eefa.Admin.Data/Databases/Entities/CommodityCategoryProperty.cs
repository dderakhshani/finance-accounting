using System.Collections.Generic;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class CommodityCategoryProperty : BaseEntity
    {


        /// <summary>
        /// کد
        /// </summary>
         

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
        /// واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        /// <summary>
        /// قوانین حاکم بر مولفه
        /// </summary>
        public string? PropertyRule { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual User CreatedBy { get; set; } = default!;
        public virtual BaseValue? Measure { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<CommodityProperty> CommodityProperties { get; set; } = default!;
    }
}
