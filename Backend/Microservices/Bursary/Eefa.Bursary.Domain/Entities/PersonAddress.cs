using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1570;&#1583;&#1585;&#1587;&#1607;&#1575;&#1740; &#1575;&#1588;&#1582;&#1575;&#1589; 
    /// </summary>
    public partial class PersonAddress : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شخص
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
//عنوان آدرس
        /// </summary>
        public int TypeBaseId { get; set; } = default!;

        /// <summary>
//آدرس
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
//کد شهر
        /// </summary>
        public int? CountryDivisionId { get; set; }

        /// <summary>
//شماره تلفن
        /// </summary>
        public string? TelephoneJson { get; set; }

        /// <summary>
//کد پستی
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
//آدرس پیش فرض 
        /// </summary>
        public bool? IsDefault { get; set; }

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
         

        public virtual CountryDivisions CountryDivision { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Persons Person { get; set; } = default!;
        public virtual BaseValues TypeBase { get; set; } = default!;
    }
}
