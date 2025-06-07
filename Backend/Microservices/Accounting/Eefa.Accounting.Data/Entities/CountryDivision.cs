using System.Collections.Generic;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class CountryDivision : BaseEntity
    {


        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        ///  کد استان
        /// </summary>
        public string? Ostan { get; set; }

        /// <summary>
        /// نام استان
        /// </summary>
        public string? OstanTitle { get; set; }

        /// <summary>
        /// کد شهرستان
        /// </summary>
        public string? Shahrestan { get; set; }

        /// <summary>
        /// نام شهرستان
        /// </summary>
        public string? ShahrestanTitle { get; set; }

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
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
    }
}
