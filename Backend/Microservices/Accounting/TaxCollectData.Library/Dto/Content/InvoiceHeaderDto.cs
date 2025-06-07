namespace TaxCollectData.Library.Dto.Content;

public record InvoiceHeaderDto
{
    /**
     * taxId
     */
    public string Taxid { get; init; }

    /**
     * invoiceDateTime
     */
    public long? Indatim { get; init; }

    /**
     * invoiceDateTimeGregorian
     */
    public long? Indati2m { get; init; }

    /**
     * invoiceType
     */
    public int? Inty { get; init; }

    /**
     * invoiceNumber
     */
    public string Inno { get; init; }

    /**
     * invoiceReferenceTaxId
     */
    public string Irtaxid { get; init; }

    /**
     * invoicePattern
     */
    public int? Inp { get; init; }

    /**
     * invoiceSubject
     */
    public int? Ins { get; init; }

    /**
     * sellerTaxIdentificationNumber
     */
    public string Tins { get; init; }

    /**
     * typeOfBuyer
     */
    public int? Tob { get; init; }

    /**
     * buyerId
     */
    public string Bid { get; init; }

    /**
     * buyerTaxIdentificationNumber
     */
    public string Tinb { get; init; }

    /**
     * sellerBranchCode
     */
    public string Sbc { get; init; }

    /**
     * buyerPostalCode
     */
    public string Bpc { get; init; }

    /**
     * buyerBranchCode
     */
    public string Bbc { get; init; }

    /**
     * flightType
     */
    public int? Ft { get; init; }

    /**
     * buyerPassportNumber
     */
    public string Bpn { get; init; }

    /**
     * sellerCustomsLicenceNumber
     */
    public string Scln { get; init; }

    /**
     * sellerCustomsCode
     */
    public string Scc { get; init; }

    /**
     * contractRegistrationNumber
     */
    public string Crn { get; init; }

    /**
     * billingId
     */
    public string Billid { get; init; }

    /**
     * totalPreDiscount
     */
    public decimal? Tprdis { get; init; }

    /**
     * totalDiscount
     */
    public decimal? Tdis { get; init; }

    /**
     * totalAfterDiscount
     */
    public decimal? Tadis { get; init; }

    /**
     * totalVatAmount
     */
    public decimal? Tvam { get; init; }

    /**
     * totalOtherDutyAmount
     */
    public decimal? Todam { get; init; }

    /**
     * totalBill
     */
    public decimal? Tbill { get; init; }

    /**
     * settlementType
     */
    public int? Setm { get; init; }

    /**
     * cashPayment
     */
    public decimal? Cap { get; init; }

    /**
     * installmentPayment
     */
    public decimal? Insp { get; init; }

    /**
     * totalVatOfPayment
     */
    public decimal? Tvop { get; init; }

    /**
     * tax17
     */
    public decimal? Tax17 { get; init; }
    
    // Export Fields
    
    /**
     * شماره کوتاژ اظهارنامه گمرکی
     */
    public string Cdcn { get; init; }
    /**
     * تاریخ کوتاژ اظهارنامه گمرکی
     */
    public int? Cdcd { get; init; }
    /**
     * مجموع وزن خالص
     */
    public decimal? Tonw { get; init; }
    /**
     * مجموع ارزش ریالی
     */
    public decimal? Torv { get; init; }
    /**
     * مجموع ارزش ارزی
     */
    public decimal? Tocv { get; init; }
    /**
     * شماره اقتصادی آژانس
     */
    public string? Tinc { get; init; }
    
}