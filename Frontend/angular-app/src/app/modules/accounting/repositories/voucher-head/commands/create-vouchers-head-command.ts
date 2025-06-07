import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {VoucherHead} from "../../../entities/voucher-head";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {CreateVoucherDetailCommand} from "../../voucher-detail/commands/create-voucher-detail-command";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class CreateVouchersHeadCommand extends IRequest<CreateVouchersHeadCommand, VoucherHead> {
  public id: number | undefined = undefined;
  public voucherDailyId: number | undefined = undefined;
  public voucherNo: number | undefined = undefined;
  public voucherDate: Date | undefined = undefined;
  public yearId: number | undefined = undefined;
  public voucherDescription: string | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public voucherStateId: number | undefined = undefined;
  public autoVoucherEnterGroup: number | undefined = undefined;

  public vouchersDetailsList: CreateVoucherDetailCommand[] = [];


  public creatorFirstName: string | undefined = undefined;
  public creatorLastName: string | undefined = undefined;

  get creator() {
    return [this.creatorFirstName, this.creatorLastName].join(' ')
  }

  public debit: number | undefined = undefined;
  public credit: number | undefined = undefined;
  public remain: number | undefined = undefined;


  get url(): string {
    return '/accounting/VouchersHead/add';
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

  mapFrom(entity: VoucherHead): CreateVouchersHeadCommand {
    // @ts-ignore
    entity.id = undefined
    this.mapBasics(entity,this)
    entity.vouchersDetails.forEach(vd => {
      this.vouchersDetailsList.push(new CreateVoucherDetailCommand().mapFrom(vd))
    })
    return this;
  }

  mapFromItSelf(entity: CreateVouchersHeadCommand): CreateVouchersHeadCommand {
    // @ts-ignore
    entity.id = undefined
    this.mapBasics(entity,this)

    return this;
  }

  mapTo(): VoucherHead {
    return new VoucherHead();
  }
}


@MediatorHandler(CreateVouchersHeadCommandHandler.name)
export class CreateVouchersHeadCommandHandler implements IRequestHandler<CreateVouchersHeadCommand, VoucherHead> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateVouchersHeadCommand): Promise<VoucherHead> {
    let httpRequest = new HttpRequest<CreateVouchersHeadCommand>(request.url, request);
    return await this._httpService.Post<any, ServiceResult<VoucherHead>>(httpRequest).toPromise().then(res => {
      this._notificationService.showSuccessMessage();
      return res.objResult
    });
  }
}
