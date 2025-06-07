import { InvoiceItem } from "./invoice-item";


export class invoice {
  public id!: number;
  public codeVoucherGroupId!: number;
  public codeVoucherGroupTitle!: string;
  public yearId!: number;
  public accountReferencesGroupId!: number;
  public warehouseId!: number;
  public warehouseTitle!: string;
  public parentId!: number;
  
  public documentNo!: number;
  public documentDate!: Date;
  public expireDate!: Date;
  public documentDescription!: string;
  public documentStateBaseId!: number;
  public isManual!: boolean;
  public voucherHeadId!: number;
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
  public priceMinusDiscount!: number;
  public priceMinusDiscountPlusTax!: number;
  public paymentTypeBaseId!: number;
  public partNumber!: string;
  public requestNo!: string;
  public invoiceNo!: string;
  public documentStateBaseTitle!: string;

  //---------------------درصد مالیات ارزش افزوده-----
  public vatPercentage!: number;
  
  //-------------------------------------------------
  public isPlacementComplete!: boolean;
  //------------------تامین کننده -------------------
  public creditAccountReferenceId!: number;
  public creditAccountReferenceGroupId!: number;
  public creditReferenceTitle!: string;
  //----------------------درخواست دهنده--------------
  public requesterReferenceId!: number;
  public requesterReferenceTitle!: string;

  //----------------------پیگیری کننده---------------
  public followUpReferenceId!: number;
  public followUpReferenceTitle!: string;

  public corroborantReferenceId!: number;

  public corroborantReferenceTitle!: number;
  public tag!: string;
  public tagArray!: string[];
  public tagClass:TagClass[]=[]
  //--------------------Commodity----------------------
  public commodityTitle!: string;
  public commodityCode!: string;
  public quantity!: number;
  public itemUnitPrice!: number;

  public items!: InvoiceItem[];
  //---------------------------------------------------

  public documentHeadExtend!: DocumentHeadExtend
  public selected: boolean = false
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
