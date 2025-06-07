using Library.Common;
using System;
using System.Collections.Generic;

namespace Eefa.Accounting.Data.Entities
{
    public partial class MoadianInvoiceHeader : BaseEntity
    {
        public int? VerificationCodeId { get; set; }

        public long ListNumber { get; set; }
        public string? Errors { get; set; }
        public string? Status { get; set; } = default!;
        public string? ReferenceId { get; set; }
        public string? UId { get; set; }
        public int? PersonId { get; set; }
        public int? CustomerId { get; set; }
        public int? AccountReferenceId { get; set; }
        public bool IsSandbox { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        /// <summary>
        /// شماره منحصر به فرد مالیاتی                                                       
        /// </summary>
        public string TaxId { get; set; } = default!;
        /// <summary>
        /// تاریخ و زمان صدور صورتحساب به میلادی                                                         
        /// </summary>
        public long? Indatim { get; set; } = default!;
        /// <summary>
        /// تاریخ و زمان ایجاد صورتحساب به میلادی                                                        
        /// </summary>
        public long? Indati2m { get; set; } = default!;
        /// <summary>
        /// نوع صورتحساب                                                         
        /// </summary>
        public int? Inty { get; set; } = default!;
        /// <summary>
        /// سریال صورتحساب                                                       
        /// </summary>
        public string Inno { get; set; } = default!;
        public string InvoiceNumber { get; set; } = default!;
        /// <summary>
        /// شماره منحصر به فرد مالیاتی صورتحساب مرجع                                                         
        /// </summary>
        public string? Irtaxid { get; set; }
        /// <summary>
        /// الگوی صورتحساب                                                       
        /// </summary>
        public int? Inp { get; set; } = default!;
        /// <summary>
        /// موضوع صورتحساب                                                       
        /// </summary>
        public int? Ins { get; set; } = default!;
        /// <summary>
        /// شماره اقتصادی فروشنده                                                        
        /// </summary>
        public string Tins { get; set; } = default!;
        /// <summary>
        /// نوع شخص خریدار                                                       
        /// </summary>
        public int? Tob { get; set; }
        /// <summary>
        /// شماره/شناسه ملی/شناسه مشارکت مدنی/کد فراگیر خریدار                                                       
        /// </summary>
        public string? Bid { get; set; }
        /// <summary>
        /// شماره اقتصادی خریدار                                                         
        /// </summary>
        public string? Tinb { get; set; }
        /// <summary>
        /// کد شعبه فروشنده                                                      
        /// </summary>
        public string? Sbc { get; set; }
        /// <summary>
        /// کد پستی خریدار                                                       
        /// </summary>
        public string? Bpc { get; set; }
        /// <summary>
        /// کد شعبه خریدار                                                       
        /// </summary>
        public string? Bbc { get; set; }
        /// <summary>
        /// نوع پرواز                                                        
        /// </summary>
        public int? Ft { get; set; }
        /// <summary>
        /// شماره گذرنامه خریدار                                                         
        /// </summary>
        public string? Bpn { get; set; }
        /// <summary>
        /// شماره پروانه گمرکی فروشنده                                                       
        /// </summary>
        public string? Scln { get; set; }
        /// <summary>
        /// کد گمرک محل اظهار                                                        
        /// </summary>
        public string? Scc { get; set; }
        /// <summary>
        /// شناسه یکتای ثبت قرارداد فروشنده                                                      
        /// </summary>
        public string? Crn { get; set; }
        /// <summary>
        /// شماره اشتراک/ شناسه قبض بهره بردار                                                       
        /// </summary>
        public string? Billid { get; set; }
        /// <summary>
        /// مجموع مبلغ قبل از کسر تخفیف                                                      
        /// </summary>
        public decimal? Tprdis { get; set; } = default!;
        /// <summary>
        /// مجموع تخفیفات                                                        
        /// </summary>
        public decimal? Tdis { get; set; } = default!;
        /// <summary>
        /// مجموع مبلغ پس از کسر تخفیف                                                       
        /// </summary>
        public decimal? Tadis { get; set; } = default!;
        /// <summary>
        /// مجموع مالیات بر ارزش افزوده                                                      
        /// </summary>
        public decimal? Tvam { get; set; } = default!;
        /// <summary>
        /// مجموع سایر مالیات، عوارض و وجوه قانونی                                                       
        /// </summary>
        public decimal? Todam { get; set; } = 0!;
        /// <summary>
        /// مجموع صورتحساب                                                       
        /// </summary>
        public decimal? Tbill { get; set; } = default!;
        /// <summary>
        /// روش تسویه                                                        
        /// </summary>
        public int? Setm { get; set; } = default!;
        /// <summary>
        /// مبلغ پرداختی نقدی                                                        
        /// </summary>
        public decimal? Cap { get; set; } = default!;
        /// <summary>
        /// مبلغ پرداخت نسیه                                                         
        /// </summary>
        public decimal? Insp { get; set; } = default!;
        /// <summary>
        /// مجموع سهم مالیات بر ارزش افزوده از پرداخت                                                        
        /// </summary>
        public decimal? Tvop { get; set; } = default!;
        /// <summary>
        /// 17 مالیات موضوع ماده                                                         
        /// </summary>
        public decimal? Tax17 { get; set; } = default!;
        /// <summary>
        /// شماره کوتاژ اظهارنامه گمرکی                                                      
        /// </summary>
        public string? Cdcn { get; set; }
        /// <summary>
        /// تاریخ کوتاژ اظهارنامه گمرکی                                                      
        /// </summary>
        public int? Cdcd { get; set; }
        /// <summary>
        /// مجموع وزن خالص                                                       
        /// </summary>
        public decimal? Tonw { get; set; } = default!;
        /// <summary>
        /// مجموع ارزش ریالی                                                         
        /// </summary>
        public decimal? Torv { get; set; } = default!;
        /// <summary>
        /// مجموع ارزش ارزی                                                      
        /// </summary>
        public decimal? Tocv { get; set; } = default!;

        public int YearId { get; set; }


        public virtual ICollection<MoadianInvoiceDetail> MoadianInvoiceDetails { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual Customer Customer { get; set; } = default!;
        public virtual AccountReference AccountReference { get; set; } = default!;
        public virtual VerificationCode VerificationCode { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;

    }
}
