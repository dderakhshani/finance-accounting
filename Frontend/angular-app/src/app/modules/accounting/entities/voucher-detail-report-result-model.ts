export class VoucherDetailReportResultModel {
  public title!:string;
  public code!:string;
  public id!: number;
  public voucherId!: number;
  public voucherNumber!: number;
  public date!: Date;
  public description!: string;
  public credit!: number;
  public debit!: number;
  public remain!: number;

  public currencyCredit!: number;
  public currencyDebit!: number;
  public currencyRemain!: number;
  public currencyFee!: number;
  public currencyTypeBaseId!: number;

  public accountHeadId!: number;
  public accountReferenceGroupId!: number;
  public accountReferenceId!: number;
}
