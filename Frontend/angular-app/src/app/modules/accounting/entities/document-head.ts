import {DocumentItem} from "./document-item";

export class DocumentHead {
  public id!:number;
  public codeVoucherGroupId!:number;
  public yearId!:number;
  public accountReferencesGroupId!:number;
  public warehouseId!:number;
  public parentId!:number;
  public referenceId!:number;
  public documentNo!:number;
  public documentDate!:Date;
  public expireDate!:Date;
  public documentDescription!:string;
  public documentStateBaseId!:number;
  public isManual!:boolean;
  public voucherHeadId!:number;
  public totalItemPrice!:number;
  public vatTax!:number;
  public vatDutiesTax!:number;
  public healthTax!:number;
  public totalItemsDiscount!:number;
  public discountPercent!:number;
  public documentDiscount!:number;
  public totalWeight!:number;
  public totalQuantity!:number;
  public priceMinusDiscount!:number;
  public priceMinusDiscountPlusTax!:number;
  public paymentTypeBaseId!:number;
  public partNumber!:string;
  public items!:DocumentItem[];




}
