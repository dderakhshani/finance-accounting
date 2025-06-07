import {VoucherDetail} from "./voucher-detail";
import {AuditableEntity} from "../../../core/abstraction/auditable-entity";
import {VoucherAttachment} from "./voucher-attachment";

export class VoucherHead extends AuditableEntity {
  public companyId:number = 0;
  public voucherDailyId:number = 0;
  public yearId:number = 0;
  public voucherNo:number = 0;
  public traceNumber:number = 0;
  public voucherDate:Date = new Date();
  public voucherDescription:string = '';
  public codeVoucherGroupId:number = 0;
  public voucherStateId:number = 0;
  public voucherStateName:string = '';
  public autoVoucherEnterGroup:number = 0;
  public totalDebit:number = 0;
  public totalCredit:number = 0;
  public difference:number = 0;
  public creator:string = '';
  public modifier:string = '';
  public codeVoucherGroupTitle:string = '';
  public hasCorrectionRequest:boolean = false;
  public vouchersDetails:VoucherDetail[] = [];
  public voucherAttachmentsList:VoucherAttachment[] = [];
  public attachments: any[] = [];


}
