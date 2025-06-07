using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1588;&#1582;&#1575;&#1589;
    /// </summary>
    public partial class Persons : BaseEntity
    {
        public Persons()
        {
            MoadianInvoiceHeaders = new HashSet<MoadianInvoiceHeaders>();
            PersonAddresses = new HashSet<PersonAddress>();
            PersonBankAccounts = new HashSet<PersonBankAccounts>();
            PersonPhones = new HashSet<PersonPhones>();
            Users = new HashSet<Users>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//نام
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
//نام خانوادگی
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
//نام پدر
        /// </summary>
        public string? FatherName { get; set; }

        /// <summary>
//کد اقتصادی مشتری
        /// </summary>
        public string? EconomicCode { get; set; }

        /// <summary>
//کد ملی
        /// </summary>
        public string NationalNumber { get; set; } = default!;

        /// <summary>
//شماره شناسنامه
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
//شماره بیمه
        /// </summary>
        public string? InsuranceNumber { get; set; }

        /// <summary>
//شماره موبایل
        /// </summary>
        public string? MobileJson { get; set; }

        /// <summary>
//ایمیل
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
//لینک عکس شخص
        /// </summary>
        public string? PhotoURL { get; set; }

        /// <summary>
//لینک امضا
        /// </summary>
        public string? SignatureURL { get; set; }

        /// <summary>
//کد طرف حساب
        /// </summary>
        public int? AccountReferenceId { get; set; }

        /// <summary>
//تاریخ تولد
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
//شهر محل تولد
        /// </summary>
        public int? BirthPlaceCountryDivisionId { get; set; }
        public bool TaxIncluded { get; set; } = default!;

        /// <summary>
//جنسیت
        /// </summary>
        public int GenderBaseId { get; set; } = default!;

        /// <summary>
//حقیقی/ حقوقی
        /// </summary>
        public int? LegalBaseId { get; set; }

        /// <summary>
//دولتی/ غیر دولتی
        /// </summary>
        public int? GovernmentalBaseId { get; set; }

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
         

        public virtual AccountReferences AccountReference { get; set; } = default!;
        public virtual BaseValues GenderBase { get; set; } = default!;
        public virtual BaseValues GovernmentalBase { get; set; } = default!;
        public virtual BaseValues LegalBase { get; set; } = default!;
        public virtual ICollection<MoadianInvoiceHeaders> MoadianInvoiceHeaders { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<PersonBankAccounts> PersonBankAccounts { get; set; } = default!;
        public virtual ICollection<PersonPhones> PersonPhones { get; set; } = default!;
        public virtual ICollection<Users> Users { get; set; } = default!;
    }
}
