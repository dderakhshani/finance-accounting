using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{

    public partial class PersonAddress : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد والد
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// عنوان آدرس
        /// </summary>
        public int TypeBaseId { get; set; } = default!;

        /// <summary>
        /// آدرس
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// کد شهرستان
        /// </summary>
        public int? CountryDivisionId { get; set; }

        /// <summary>
        /// تلفن
        /// </summary>
        public string? TelephoneJson { get; set; }

        /// <summary>
        /// کد پستی
        /// </summary>
        public string? PostalCode { get; set; }

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
        

        public virtual CountryDivision? CountryDivision { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual BaseValue TypeBase { get; set; } = default!;
    }
}
