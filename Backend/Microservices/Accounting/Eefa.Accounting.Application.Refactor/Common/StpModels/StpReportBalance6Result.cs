public class StpReportBalance6Result : PermissionForListModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? CodeTitle { get; set; }
    public string? Title { get; set; }
    public double? DebitBeforeDate { get; set; }
    public double? CreditBeforeDate { get; set; }
    public double? Debit { get; set; }
    public double? Credit { get; set; }
    public double? RemainDebit { get; set; }
    public double? RemainCredit { get; set; }
    public double? DebitAfterDate { get; set; }
    public double? CreditAfterDate { get; set; }

    public double? DebitCurrencyAmountBefore { get; set; }
    public double? CreditCurrencyAmountBefore { get; set; }
    public double? DebitCurrencyAmount { get; set; }
    public double? CreditCurrencyAmount { get; set; }
    public double? DebitCurrencyRemain { get; set; }
    public double? CreditCurrencyRemain { get; set; }
    public double? DebitCurrencyAmountAfter { get; set; }
    public double? CreditCurrencyAmountAfter { get; set; }
}