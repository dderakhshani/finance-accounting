import { DocumentHead } from "./document-head"
import { FinancialRequestDetail } from "./financial-detail"

export class RequestPayment {

  public id!: number
  public parentId : number | undefined = undefined
  public codeVoucherGroupId : number | undefined = undefined
  public yearId : number | undefined = undefined
  public paymentTypeBaseId : number | undefined = undefined
  public voucherHeadId : number | undefined = undefined
  public workflowId : number | undefined = undefined
  public financialStatusBaseId : number | undefined = undefined
  public documentNo : string | undefined = undefined
  public documentSerial : string | undefined = undefined
  public documentDate : Date | undefined = undefined
  public description : string | undefined = undefined
  public paymentStatus : number | undefined = undefined
  public amount !: number
  public expireDate : Date | undefined = undefined
  public totalAmount : number | undefined = undefined
  public deductAmount : number | undefined = undefined
  public deductionReason : string | undefined = undefined
  public issueDate : Date | undefined = undefined
  public extraFieldJson : string | undefined = undefined
  public missedDocumentJson : string | undefined = undefined
  public workflowState : string | undefined = undefined
  public isEmergent : boolean | undefined = undefined
  public isAccumulativePayment : boolean | undefined = undefined
  public creditAccountReferenceTitle : string | undefined = undefined
  public debitAccountReferenceTitle : string | undefined = undefined
  public creditAccountReferenceCode : string | undefined = undefined
  public creditAccountReferenceGroupTitle : string | undefined = undefined
  public creditAccountReferenceGroupCode : string | undefined = undefined
  public debitAccountReferenceGroupCode : string | undefined = undefined
  public debitAccountReferenceCode : string | undefined = undefined
  public documentHeads: DocumentHead[] = []
  public financialRequestDetails : FinancialRequestDetail[] =[];
  public voucherHeadCode : number | undefined = undefined;
  public detailDescription : string | undefined = undefined;
  public createName : string | undefined = undefined;
    public documentTypeBaseTitle : string | undefined = undefined;
  public voucherStateId : number | undefined = undefined;
public currencyTypeBaseTitle : string | undefined = undefined;
public currencyFee : number | undefined = undefined;
public currencyAmount :number | undefined = undefined;



}
