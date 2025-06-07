using System;
using System.Collections.Generic;
using Library.Common;

namespace Eefa.Audit.Data.Databases.Entities
{
    public partial class Person : BaseEntity
    {
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
        /// نام پدر
        /// </summary>
        public string FatherName { get; set; } = default!;

        /// <summary>
        /// کد ملی
        /// </summary>
        public string NationalNumber { get; set; } = default!;

        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// شماره بیمه
        /// </summary>
        public string? InsuranceNumber { get; set; }
        public string? MobileJson { get; set; } = default!;

        /// <summary>
        /// ایمیل
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// لینک عکس شخص
        /// </summary>
        public string? PhotoURL { get; set; }

        /// <summary>
        /// لینک امضا
        /// </summary>
        public string? SignatureURL { get; set; }

        /// <summary>
        /// کد طرف حساب
        /// </summary>
        public int? AccountReferenceId { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// محل تولد
        /// </summary>
        public int? BirthPlaceCountryDivisionId { get; set; }

        /// <summary>
        /// جنسیت
        /// </summary>
        public int GenderBaseId { get; set; } = default!;

        /// <summary>
        /// حقیقی/ حقوقی
        /// </summary>
        public int? LegalBaseId { get; set; }

        /// <summary>
        /// دولتی/ غیر دولتی
        /// </summary>
        public int? GovernmentalBaseId { get; set; }


        public virtual AccountReference? AccountReference { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual BaseValue GenderBase { get; set; } = default!;
        public virtual BaseValue? GovernmentalBase { get; set; } = default!;
        public virtual BaseValue? LegalBase { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Employee? Employee { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<PersonFingerprint> PersonFingerprints { get; set; } = default!;
        public virtual ICollection<Signer> Signers { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;
    }
}
