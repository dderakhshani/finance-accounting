import {IRequest} from "../../../../../core/services/mediator/interfaces";
import {VoucherHead} from "../../../entities/voucher-head";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {VoucherDetail} from "../../../entities/voucher-detail";

export class CreateVoucherDetailCommand extends IRequest<CreateVoucherDetailCommand,VoucherDetail>{
  constructor() {
    super();
  }
  public id: number | undefined = undefined;
  public documentNo: number | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public voucherDate:Date | undefined = undefined;
  public accountHeadId:number | undefined = undefined;
  public accountHeadCode:string | undefined = undefined;
  public voucherRowDescription:string | undefined = undefined;
  public debit:number | undefined = undefined;
  public credit:number | undefined = undefined;
  public rowIndex:number | undefined = undefined;
  public documentId:number | undefined = undefined;
  public financialOperationNumber:string | undefined = undefined;
  public referenceId:number | undefined = undefined;
  public referenceDate:Date | undefined = undefined;
  public referenceQty:number | undefined = undefined;
  public accountReferencesGroupId:number | undefined = undefined;
  public referenceId1:number | undefined = undefined;
  public referenceTitle1:number | undefined = undefined;
  public referenceCode1:number | undefined = undefined;
  public referenceId2:number | undefined = undefined;
  public referenceTitle2:number | undefined = undefined;
  public referenceId3:number | undefined = undefined;
  public referenceTitle3:number | undefined = undefined;
  public level1:number | undefined = undefined;
  public level2:number | undefined = undefined;
  public level3:number | undefined = undefined;
  public currencyFee:number | undefined = undefined;
  public currencyTypeBaseId:number | undefined = undefined;
  public currencyAmount:number | undefined = undefined;
  public traceNumber:number | undefined = undefined;
  public quantity:number | undefined = undefined;
  public weight:number | undefined = undefined;
  public invoiceNo:string | undefined = undefined;
  public createdBy:string | undefined = undefined;
  public modifiedBy:string | undefined = undefined;

  get url(): string {
    return "/accounting/voucherdetail/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

  mapFrom(entity: VoucherDetail): CreateVoucherDetailCommand {
    // @ts-ignore
    entity.id = undefined
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): VoucherDetail {
    return new VoucherDetail();
  }
}
