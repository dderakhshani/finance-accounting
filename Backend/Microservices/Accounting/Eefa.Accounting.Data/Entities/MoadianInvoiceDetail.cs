using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class MoadianInvoiceDetail : BaseEntity
    {
        public int InvoiceHeaderId { get; set; } = default!;
        /// <summary>
        /// شناسه خدمت/کالا                                                       
        /// </summary>
        public string Sstid { get; set; } = default!;
        /// <summary>
        /// شرح خدمت/کالا                                                        
        /// </summary>
        public string Sstt { get; set; } = default!;
        /// <summary>
        /// واحد اندازه گیری                                                         
        /// </summary>
        public string Mu { get; set; } = default!;
        /// <summary>
        /// تعداد/مقدار                                                      
        /// </summary>
        public decimal? Am { get; set; } = default!;
        /// <summary>
        /// مبلغ واحد                                                        
        /// </summary>
        public decimal? Fee { get; set; } = default!;
        /// <summary>
        /// میزان ارز                                                        
        /// </summary>
        public decimal? Cfee { get; set; } = default!;
        /// <summary>
        /// نوع ارز                                                      
        /// </summary>
        public string? Cut { get; set; }
        /// <summary>
        /// نرخ برابری ارز با ریال                                                       
        /// </summary>
        public decimal? Exr { get; set; } = default!;
        /// <summary>
        /// مبلغ قبل از تخفیف                                                        
        /// </summary>
        public decimal? Prdis { get; set; } = default!;
        /// <summary>
        /// مبلغ تحفیف                                                       
        /// </summary>
        public decimal? Dis { get; set; } = default!;
        /// <summary>
        /// مبلغ بعد از تخفیف                                                        
        /// </summary>
        public decimal? Adis { get; set; } = default!;
        /// <summary>
        /// نرخ مالیات بر ارزش افزوده                                                        
        /// </summary>
        public decimal? Vra { get; set; } = default!;
        /// <summary>
        /// مبلغ مالیات بر ارزش افزوده                                                       
        /// </summary>
        public decimal? Vam { get; set; } = default!;
        /// <summary>
        /// موضوع سایرمالیات و عوارض                                                         
        /// </summary>
        public string? Odt { get; set; }
        /// <summary>
        /// نرخ سایرمالیات و عوارض                                                       
        /// </summary>
        public decimal? Odr { get; set; } = default!;
        /// <summary>
        /// مبلغ سایرمالیات و عوارض                                                      
        /// </summary>
        public decimal? Odam { get; set; } = default!;
        /// <summary>
        /// موضوع سایر وجوه قانونی                                                       
        /// </summary>
        public string? Olt { get; set; }
        /// <summary>
        /// نرخ سایر وجوه قانونی                                                         
        /// </summary>
        public decimal? Olr { get; set; } = default!;
        /// <summary>
        /// مبلغ سایر وجوه قانونی                                                        
        /// </summary>
        public decimal? Olam { get; set; } = default!;
        /// <summary>
        /// اجرت ساخت                                                        
        /// </summary>
        public decimal? Consfee { get; set; } = default!;
        /// <summary>
        /// سود فروشنده                                                      
        /// </summary>
        public decimal? Spro { get; set; } = default!;
        /// <summary>
        /// حق العمل                                                         
        /// </summary>
        public decimal? Bros { get; set; } = default!;
        /// <summary>
        /// جمع کل اجرت، حق العمل و سود                                                          
        /// </summary>
        public decimal? Tcpbs { get; set; } = default!;
        /// <summary>
        /// سهم نقدی از پرداخت                                                       
        /// </summary>
        public decimal? Cop { get; set; } = default!;
        /// <summary>
        /// سهم ارزش افزوده از پرداخت                                                        
        /// </summary>
        public decimal? Vop { get; set; } = default!;
        /// <summary>
        /// شناسه یکتای ثبت قرارداد حق العمل کاری                                                        
        /// </summary>
        public string? Bsrn { get; set; }
        /// <summary>
        ///  مبلغ کل کالا/خدمت                                                       
        /// </summary>
        public decimal? Tsstam { get; set; } = default!;
        /// <summary>
        ///وزن خالص
        /// </summary>
        public decimal? Nw { get; set; } = default!;
        /// <summary>
        /// ارزش ریالی کالا                                                      
        /// </summary>
        public decimal? Sscv { get; set; } = default!;
        /// <summary>
        /// ارزش ارزی کالا                                                       
        public decimal? Ssrv { get; set; } = default!;
        /// </summary>       /// <summary>
        /// تفاوت نرخ خرید و فروش ارز/ کارمزد فروش ارز
        /// </summary>
        public decimal? Pspd { get; set; } = default!;
        public virtual MoadianInvoiceHeader InvoiceHeader { get; set; } = default!;

    }
}
