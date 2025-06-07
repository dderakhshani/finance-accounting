import { CorrectionRequestModel } from "./correctionRequestModel";
import { ReceiptItem } from "./receipt-item";

export class Receipt {
  public id!: number;

  public warehouseId!: number;
  public warehouseTitle!: string;
  public serialNumber!: number;
  public documentNo!: number;
  public documentId!: number;
  public requestNo!: string;
  public invoiceNo!: string;
  public referenceTitle!: string;
  public documentDate!: Date;
  public expireDate!: Date;
  public requestDate!: Date;
  public invoiceDate!: Date;
  public modifiedAt!: Date;


  public documentDescription!: string;
  public documentStateBaseId!: number;
  public isManual!: boolean;
  public voucherHeadId!: any;
  public totalItemPrice!: number;
  public vatTax!: number;
  public vatDutiesTax!: number;
  public healthTax!: number;
  public totalItemsDiscount!: number;
  public totalProductionCost!: number;
  public discountPercent!: number;
  public documentDiscount!: number;
  public totalWeight!: number;
  public totalQuantity!: number;
  public extraCost!: number;
  public priceMinusDiscount!: number;
  public priceMinusDiscountPlusTax!: number;
  public paymentTypeBaseId!: number;
  public partNumber!: string;
  public voucherNo!: string;
  public measureTitle!: string;
  public documentStateBaseTitle!: string;
  public financialOperationNumber!: string;
  public username !: string;
  public parentId!: number;
  public yearId!: number;
  public yearName!: string;



  public documentIds!: string;
  public printCount!: number;
  public rowsCount!: number;
  //-------------------codeVoucherGroup---------------
  public codeVoucherGroupId!: number;
  public codeVoucherGroupTitle!: string;
  public documentStauseBaseValue!: number;
  public documentStauseBaseValueTitle!: string;
  public viewId!: number;
  //---------------------درصد مالیات ارزش افزوده-----
  public vatPercentage!: number;
  //--------------------------------------------------
  public isPlacementComplete!: boolean;
  public isImportPurchase!: boolean;
  //------------------تامین کننده -------------------
  public debitAccountHeadId !: number;
  public debitAccountReferenceId !: number;
  public debitAccountReferenceGroupId !: number;
  public creditAccountHeadId !: number;
  public creditAccountReferenceId !: number;
  public creditAccountReferenceGroupId !: number;
  public creditReferenceTitle!: string;
  public debitReferenceTitle!: string;
  public defultCreditAccountHeadId !: number;
  public defultDebitAccountHeadId !: number;
  public debitAccountHeadTitle!: string;
  public creditAccountHeadTitle!: string;
  public debitAccountReferenceGroupTitle!: string;
  public creditAccountReferenceGroupTitle!: string;

  //----------------------درخواست دهنده--------------
  public requesterReferenceId!: number;
  public requesterReferenceTitle!: string;

  //----------------------پیگیری کننده---------------
  public followUpReferenceId!: number;
  public followUpReferenceTitle!: string;
  public corroborantReferenceId!: number;
  public corroborantReferenceTitle!: number;

  //------------------------------------------------
  public commodityTitle!: string;
  public commodityCode!: string;
  public descriptionItem!: string;
  public quantity!: number;
  public remainQuantity!: number;
  public itemsCount!: number;
  public currencyRate!: number;
  public currencyPrice!: number;
  public currencyBaseId!: number;
  public currencyBaseTitle!: string;

  public extraCostAccountHeadId !: number;
  public extraCostAccountReferenceGroupId !: number;
  public extraCostAccountReferenceId !: number;
  public extraCostAccountHeadTitle !: string;

  public extraCostCurrency!: number;

  //----------------تعداد شماره سریال اموال وارد شده
  public assetsCount!: number;

  public selected: boolean = false
  public isFreightChargePaid: boolean = false
  public isDocumentIssuance: boolean = true;
  public isAllowedInputOrOutputCommodity: boolean = true;


  //----------------tags----------------------------
  public tags!: string;
  public tagArray!: string[];
  public tagClass: TagClass[] = []


  //------------------items------------------------
  public items!: ReceiptItem[];
  public documentHeadExtend!: DocumentHeadExtend
  public correctionRequest: CorrectionRequestModel[]=[]
}


export class DocumentHeadExtend {

  public documentHeadId!: number;
  //----------------------درخواست دهنده------------------
  public requesterReferenceId!: number;
  public requesterReferenceTitle!: string;
  //----------------------پیگیری کننده-------------------
  public followUpReferenceId!: number;
  public followUpReferenceTitle!: string;
  public corroborantReferenceId!: number;
  public corroborantReferenceTitle!: number;
}
export class TagClass {
  public key!: string;
}
export class Quantities {

  public quantity!: number;
}
export interface View_Sina_FinancialOperationNumber {
  id: number;
  paymentNumber: string;
}
