namespace TaxCollectData.Library.Dto.Content;

public class InvoiceBodyDto
{
    /**
     * service Stuff Id
     */
    public string Sstid { get; init; }

    /**
     * serviceStuffTitle
     */
    public string Sstt { get; init; }

    /**
     * measurementUnit
     */
    public string Mu { get; init; }

    /**
     * amount
     */
    public decimal? Am { get; init; }

    /**
     * fee
     */
    public decimal? Fee { get; init; }

    /**
     * currencyFee
     */
    public decimal? Cfee { get; init; }

    /**
     * currencyType
     */
    public string Cut { get; init; }

    /**
     * exchangeRate
     */
    public decimal? Exr { get; init; }

    /**
     * preDiscount
     */
    public decimal? Prdis { get; init; }

    /**
     * discount
     */
    public decimal? Dis { get; init; }

    /**
     * afterDiscount
     */
    public decimal? Adis { get; init; }

    /**
     * vatRate
     */
    public decimal? Vra { get; init; }

    /**
     * vatAmount
     */
    public decimal? Vam { get; init; }

    /**
     * overDutyTitle
     */
    public string Odt { get; init; }

    /**
     * overDutyRate
     */
    public decimal? Odr { get; init; }

    /**
     * overDutyAmount
     */
    public decimal? Odam { get; init; }

    /**
     * otherLegalTitle
     */
    public string Olt { get; init; }

    /**
     * otherLegalRate
     */
    public decimal? Olr { get; init; }

    /**
     * otherLegalAmount
     */
    public decimal? Olam { get; init; }

    /**
     * constructionFee
     */
    public decimal? Consfee { get; init; }

    /**
     * sellerProfit
     */
    public decimal? Spro { get; init; }

    /**
     * brokerSalary
     */
    public decimal? Bros { get; init; }

    /**
     * totalConstructionProfitBrokerSalary
     */
    public decimal? Tcpbs { get; init; }

    /**
     * cashOfPayment
     */
    public decimal? Cop { get; init; }

    /**
     * vatOfPayment
     */
    public decimal? Vop { get; init; }

    /**
     * buyerSRegisterNumber
     */
    public string Bsrn { get; init; }

    /**
     * totalServiceStuffAmount
     */
    public decimal? Tsstam { get; init; }
    
    // Export Fields
    
    /**
     * وزن خالص
     */
    public decimal? Nw { get; init; }
    /**
     * ارزش ریالی کالا
     */
    public decimal? Ssrv { get; init; }
    /**
     * ارزش ارزی کالا
     */
    public decimal? Sscv { get; init; }
    /**
    * تفاوت نرخ خرید و فروش ارز/کارمزد فروش ارز
    */
    public decimal? pspd { get; init; }
    /**
    * عیار طلا
    */
    public decimal? cui { get; init; }
    
}