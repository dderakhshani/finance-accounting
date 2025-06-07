using System.Collections.Generic;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class BaseValue : BaseEntity
    {
      
        public int BaseValueTypeId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// شناسه
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
        /// نام یکتا
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// مقدار
        /// </summary>
        public string Value { get; set; } = default!;

        /// <summary>
        /// ترتیب آرتیکل سند حسابداری
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

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
        

        public virtual BaseValueType BaseValueType { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual ICollection<Attachment> Attachments { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
        public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = default!;
        //public virtual ICollection<Commodity> CommodityMeasures { get; set; } = default!;
       // public virtual ICollection<Commodity> CommodityPricingTypeNavigations { get; set; } = default!;
        public virtual ICollection<CommodityProperty> CommodityProperties { get; set; } = default!;
        //public virtual ICollection<DocumentHead> DocumentHeadInvoiceTypes { get; set; } = default!;
        //public virtual ICollection<DocumentHead> DocumentHeadPaymentTypes { get; set; } = default!;
        //public virtual ICollection<DocumentHead> DocumentHeadTypes { get; set; } = default!;
        public virtual ICollection<DocumentHead> DocumentHeads { get; set; } = default!;
        public virtual ICollection<Language> Languages { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<PersonFingerprint> PersonFingerprints { get; set; } = default!;
        public virtual ICollection<Person> PersonGenderBases { get; set; } = default!;
        public virtual ICollection<Person> PersonGovernmentalBases { get; set; } = default!;
        public virtual ICollection<Person> PersonLegalBases { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;

        public virtual ICollection<Customer> Customers { get; set; } = default!;

        //public virtual ICollection<DocumentHead> DocumentHeads { get; set; } = default!;

    }
}
