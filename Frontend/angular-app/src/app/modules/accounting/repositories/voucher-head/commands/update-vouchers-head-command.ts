import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {VoucherHead} from "../../../entities/voucher-head";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {UpdateVoucherDetailCommand} from "../../voucher-detail/commands/update-voucher-detail-command";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {CreateVoucherDetailCommand} from "../../voucher-detail/commands/create-voucher-detail-command";
import {VoucherAttachment} from "../../../entities/voucher-attachment";

export class UpdateVouchersHeadCommand extends IRequest<UpdateVouchersHeadCommand, VoucherHead> {
  constructor() {
    super();
  }

  public id: number | undefined = undefined;
  public voucherDailyId: number | undefined = undefined;
  public voucherNo: number | undefined = undefined;
  public voucherDate: Date | undefined = undefined;
  public yearId: number | undefined = undefined;
  public voucherDescription: string | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public voucherStateId: number | undefined = undefined;
  public autoVoucherEnterGroup: number | undefined = undefined;
  public hasCorrectionRequest: boolean | undefined = undefined;

  public vouchersDetailsList: UpdateVoucherDetailCommand[] = [];

  public vouchersDetailsCreatedList: CreateVoucherDetailCommand[] = [];
  public vouchersDetailsUpdatedList: UpdateVoucherDetailCommand[] = [];
  public vouchersDetailsDeletedList: UpdateVoucherDetailCommand[] = [];
  public voucherAttachmentsList: VoucherAttachment[] = [];

  public creator: string | undefined = undefined;
  public modifier: string | undefined = undefined;




  public debit: number | undefined = undefined;
  public credit: number | undefined = undefined;
  public remain: number | undefined = undefined;

  get url(): string {
    return '/accounting/VouchersHead/update';
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

  mapFrom(entity: VoucherHead): UpdateVouchersHeadCommand {

    this.mapBasics(entity, this);
    entity.vouchersDetails?.forEach(voucherDetail => {
      this.vouchersDetailsList.push(new UpdateVoucherDetailCommand().mapFrom(voucherDetail))
    });
    this.voucherAttachmentsList = entity.voucherAttachmentsList
    return this
  }

  mapTo(): VoucherHead {
    return new VoucherHead();
  }
}


@MediatorHandler(UpdateVouchersHeadCommandHandler.name)
export class UpdateVouchersHeadCommandHandler implements IRequestHandler<UpdateVouchersHeadCommand, VoucherHead> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateVouchersHeadCommand): Promise<VoucherHead> {
    let httpRequest = new HttpRequest<UpdateVouchersHeadCommand>(request.url, request);
    return await this._httpService.Put<any, ServiceResult<VoucherHead>>(httpRequest).toPromise().then(res => {
      if(res.objResult.voucherStateId == 1) this._notificationService.showSuccessMessage();
      else this._notificationService.showWarningMessage('درخواست تغییرات ارسال گردید.(تغییرات شما فعلا اعمال نشده است)');
      return res.objResult
    });
  }
}
