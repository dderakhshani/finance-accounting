import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class CombineVoucherHeadsCommand extends IRequest<CombineVoucherHeadsCommand, any> {
  public mainVoucherId:number | undefined = undefined;
  public mainVoucherNo:number | undefined = undefined;
  public voucherHeadIdsToCombine:number[] = [];

  mapFrom(entity: any): CombineVoucherHeadsCommand {
    throw Error('not implemented')
  }
  mapTo(): any {
  }

  get url(): string {
    return "/accounting/vouchersHead/combine";
  }
  get validationRules(): any {
    return []
  }

}


@MediatorHandler(CombineVoucherHeadsCommandHandler.name)
export class CombineVoucherHeadsCommandHandler implements IRequestHandler<CombineVoucherHeadsCommand, any>{
  constructor(
    @Inject(HttpService) private _httpService:HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService
  ) {
  }
  async Handle(request: CombineVoucherHeadsCommand){
    let httpRequest = new HttpRequest(request.url,request);
    return this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      this._notificationService.showSuccessMessage()
      return res.objResult
    })
  }
}

