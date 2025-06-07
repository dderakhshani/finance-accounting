export class ConvertToRailsReceipt {
  public documentId: number | undefined = undefined;
  public id: number | undefined = undefined;
  public voucherHeadId: number | undefined = undefined;
  public invoiceNo: string | undefined = undefined;
  public vatDutiesTax: number | undefined = 0;
  public totalProductionCost: number | undefined = 0;
  public totalItemPrice: number | undefined = 0;
  public tags: string | undefined = undefined;
  public discountPercent: number | undefined = 0;
  public extraCost: number | undefined = 0;
  public extraCostRialTemp: number | undefined = 0;//نمایشی است ذخیره نشود.
  public extraCostCurrency: number | undefined = 0;//نمایشی است ذخیره نشود.*/
  public documentDescription: number | undefined = undefined;
  public financialOperationNumber: string | undefined = undefined;
  public invoiceDate: Date | undefined = undefined;
  //------------------تامین کننده -------------------
  public debitAccountHeadId: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public creditAccountHeadId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public extraCostAccountHeadId: number | undefined = undefined;
  public extraCostAccountReferenceGroupId: number | undefined = undefined;
  public extraCostAccountReferenceId: number | undefined = undefined;

  // receiptDocumentItems: RialsReceiptDocumentItemCommand[] = [];

  //attachmentIds: number[] | undefined = undefined;
  
}

export class RialsReceiptDocumentItemCommand {
  id: number | undefined = undefined;
  commodityId: number | undefined = undefined;
  documentHeadId: number | undefined = undefined;
  unitPrice: number | undefined = undefined;
  currencyPrice: number | undefined = undefined;
  currencyBaseId: number | undefined = undefined;
  currencyRate: number | undefined = undefined;
  description: string | undefined = undefined;
  productionCost: number | undefined = undefined;
  quantity: number | undefined = undefined;
}

export class warehouseReceiptToAutoVoucher
{
  public DocumentNo: string | undefined='';
  public DocumentDate: string | undefined = '';

  public CodeVoucherGroupId: string | undefined = '';
  public CodeVoucherGroupTitle: string | undefined = '';

  public DebitAccountHeadId: string | undefined = '';
  public DebitAccountReferencesGroupId: string | undefined = '';
  public DebitAccountReferenceId: string | undefined = '';

  public CreditAccountHeadId: string | undefined = '';
  public CreditAccountReferencesGroupId: string | undefined = '';
  public CreditAccountReferenceId: string | undefined = '';
  
  public ToTalPriceMinusVat: string | undefined = '';
  public VatDutiesTax: string | undefined = '';
  public PriceMinusDiscountPlusTax: string | undefined = '';

  public TotalQuantity: string | undefined = '';
  public TotalWeight: string | undefined = '';

  public FinancialOperationNumber: string | undefined = '';
  public InvoiceNo: string | undefined = '';
  public Tag: string | undefined = '';
  public ExtraCost: string | undefined = '';
  public DocumentId: string | undefined = '';

  public DocumentIds: string | undefined = '';
  public VocherHeadId: string | undefined;
  public VoucherRowDescription: string | undefined = '';
  public DebitAccountHeadTitle: string | undefined = '';
  public CreditAccountHeadTitle: string | undefined = '';

  public CurrencyFee: string | undefined = '';

  public CurrencyTypeBaseId: string | undefined = '';
  public CurrencyAmount: string | undefined = '';

  public ExtraCostAccountHeadId: string | undefined = '';
  public ExtraCostAccountReferenceGroupId : string | undefined = '';
  public ExtraCostAccountReferenceId: string | undefined = '';
  public ExtraCostAccountHeadTitle: string | undefined = '';
  public IsFreightChargePaid: string | undefined = '';
  public TotalItemPrice: string | undefined = '';
  
  
}
export interface AutoVoucherResults {
  objResult: AutoVoucherResult[];
  message: string;
  succeed: boolean;
}

export interface AutoVoucherResult {
  voucherHeadId: number;
  documentId: number;
  voucherNo: number;
}


