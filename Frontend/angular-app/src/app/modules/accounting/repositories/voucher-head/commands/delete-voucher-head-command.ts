import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {VoucherHead} from "../../../entities/voucher-head";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class DeleteVouchersHeadCommand extends IRequest<DeleteVouchersHeadCommand, VoucherHead> {
  constructor(public id:number) {
    super();
  }


  get url(): string {
    return '/accounting/VouchersHead/delete';
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

  mapFrom(entity: VoucherHead): DeleteVouchersHeadCommand {
    throw Error('not implemented')
  }

  mapTo(): VoucherHead {
    return new VoucherHead();
  }
}


@MediatorHandler(DeleteVouchersHeadCommandHandler.name)
export class DeleteVouchersHeadCommandHandler implements IRequestHandler<DeleteVouchersHeadCommand, VoucherHead> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteVouchersHeadCommand): Promise<VoucherHead> {
    let httpRequest = new HttpRequest<DeleteVouchersHeadCommand>(request.url, request);
    httpRequest.Query += `?id=${request.id}`;
    return await this._httpService.Delete<any>(httpRequest).toPromise().then(res => {
      this._notificationService.showSuccessMessage();
      return res.objResult
    });
  }
}
