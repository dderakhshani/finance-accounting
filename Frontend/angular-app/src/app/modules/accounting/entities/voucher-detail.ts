import {AuditableEntity} from "../../../core/abstraction/auditable-entity";

export class VoucherDetail extends AuditableEntity {
  public voucherId!:number;
  public voucherDate!:Date;
  public accountHeadId!:number;
  public accountHeadCode!:string;
  public voucherRowDescription!:string;
  public debit!:number;
  public credit!:number;
  public rowIndex!:number;
  public documentId!:number;
  public referenceId!:number;
  public referenceDate!:Date;
  public referenceQty!:number;
  public referenceId1!:number;
  public referenceCode1!:string;
  public referenceTitle1!:string;
  public referenceId2!:number;
  public referenceCode2!:string;
  public referenceTitle2!:string;
  public referenceId3!:number;
  public referenceCode3!:string;
  public referenceTitle3!:string;
  public accountReferencesGroupId!:number;
  public accountReferencesGroupTitle!:string;

  public level1!:number;
  public level2!:number;
  public level3!:number;

  public levelName1!:string;
  public levelName2!:string;
  public levelName3!:string;

  public debitCreditStatus!:number;
  public remain!:number;

  public currencyFlag!: boolean;
  public exchengeFlag!: boolean;
  public traceFlag!: boolean;
  public quantityFlag!: boolean;
  public voucherNo!: number;
  public voucherDailyId!: number;
  public invoiceNo!: number;

  public createdBy!:string;
  public modifiedBy!:string;
}
