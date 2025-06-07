import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class VoucherHeadsAdjustmentCommand extends IRequest<VoucherHeadsAdjustmentCommand,any>{
  
  mapFrom(entity: any): VoucherHeadsAdjustmentCommand {
    return new VoucherHeadsAdjustmentCommand;
  }

  mapTo(): any {
    return undefined;
  }

  get url(): string {
    return "/accounting/vouchersHead/adjustment";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(VoucherHeadsAdjustmentCommandHandler.name)
export class VoucherHeadsAdjustmentCommandHandler implements IRequestHandler<VoucherHeadsAdjustmentCommand,any>{
  constructor(
    @Inject(HttpService) private _httpService:HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService
  ) {
  }
  async Handle(request: VoucherHeadsAdjustmentCommand) {
    let httpRequest = new HttpRequest(request.url);
    return await this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      this._notificationService.showSuccessMessage();
      return res.objResult;
    })
  }
}
