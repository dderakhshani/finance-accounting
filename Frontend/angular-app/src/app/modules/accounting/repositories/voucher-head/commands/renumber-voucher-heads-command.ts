import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class RenumberVoucherHeadsCommand extends IRequest<RenumberVoucherHeadsCommand,ServiceResult<any>> {

  public codeVoucherGroupId:number | undefined = undefined;
  public voucherStateId:number | undefined = undefined;
  public fromDateTime:Date | undefined = undefined;
  public toDateTime:Date | undefined = undefined;
  public fromNo:number | undefined = undefined;
  public toNo:number | undefined = undefined;

  mapFrom(entity: any): RenumberVoucherHeadsCommand {
    return new RenumberVoucherHeadsCommand();
  }

  mapTo(): any {
    return undefined;
  }

  get url(): string {
    return "/accounting/vouchersHead/renumber";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}
@MediatorHandler(RenumberVoucherHeadsCommandHandler.name)
export class RenumberVoucherHeadsCommandHandler implements IRequestHandler<RenumberVoucherHeadsCommand,ServiceResult<any>>{
  constructor(
    @Inject(HttpService) private _httpService:HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService,
  ) {
  }
  async Handle (request: RenumberVoucherHeadsCommand) {
    let httpRequest = new HttpRequest(request.url,request);
    return this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      this._notificationService.showSuccessMessage();
      return res.objResult
    })
  }
}
