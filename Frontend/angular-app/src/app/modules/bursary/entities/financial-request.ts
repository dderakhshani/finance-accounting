import { FinancialAttachment } from "./financial-attachmen";
import { FinancialRequestDetail } from "./financial-detail";
import { FinancialPaymentPartial } from "./financial-request-partial";

 export class FinancialRequest {
  public id : number | undefined = undefined;
  public documentNo : string | undefined = undefined;
  public codeVoucherGroupId : number | undefined = undefined;
  public companyId : number | undefined = undefined;
  public documentDate : Date | undefined = undefined;
  public description : string | undefined = undefined;
  public createName : string | undefined = undefined;
  public voucherHeadId : number | undefined = undefined;
  public voucherHeadCode : number | undefined = undefined;
  public amount :number |undefined = undefined;

  public creditAccountReferenceTitle : string | undefined = undefined;
  public financialRequestDetails : FinancialRequestDetail[] = [];
  public financialRequestAttachments : FinancialAttachment [] = [];
  public financialRequestPartial : FinancialPaymentPartial [] = [];
 }
