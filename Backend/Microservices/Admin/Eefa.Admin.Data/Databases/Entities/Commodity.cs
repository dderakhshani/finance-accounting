using System.Collections.Generic;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class Commodity : BaseEntity
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
        /// کد محصول
        /// </summary>
        public string? ProductCode { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string? Tite { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// حداقل تعداد
        /// </summary>
        public double MinimumQuantity { get; set; } = default!;

        /// <summary>
        /// حداکثر تعداد
        /// </summary>
        public double? MaximumQuantity { get; set; }

        /// <summary>
        /// تعداد سفارش
        /// </summary>
        public double? OrderQuantity { get; set; }

        /// <summary>
        /// کد نوع/ باید حذف شود
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// کد طرف حساب
        /// </summary>
        public int? ReferenceId { get; set; }

        /// <summary>
        /// نوع محاسبه قیمت
        /// </summary>
        public int? PricingType { get; set; }

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
        public virtual BaseValue? PricingTypeNavigation { get; set; } = default!;
        public virtual BaseValue? Type { get; set; } = default!;
        public virtual ICollection<CommodityProperty> CommodityProperties { get; set; } = default!;
        public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
    }
}
