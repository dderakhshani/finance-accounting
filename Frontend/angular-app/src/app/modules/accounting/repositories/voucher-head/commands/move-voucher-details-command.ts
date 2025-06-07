import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class MoveVoucherDetailsCommand extends IRequest<MoveVoucherDetailsCommand, any> {
  public voucherHeadId:number | undefined = undefined;
  public voucherDetailIds:number[] = [];

  mapFrom(entity: any): MoveVoucherDetailsCommand {
    throw Error('not implemented')
  }
  mapTo(): any {
  }

  get url(): string {
    return "/accounting/vouchersHead/MoveVoucherDetails";
  }
  get validationRules(): any {
    return []
  }

}


@MediatorHandler(MoveVoucherDetailsCommandHandler.name)
export class MoveVoucherDetailsCommandHandler implements IRequestHandler<MoveVoucherDetailsCommand, any>{
  constructor(
    @Inject(HttpService) private _httpService:HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService
  ) {
  }
  async Handle(request: MoveVoucherDetailsCommand){
    let httpRequest = new HttpRequest(request.url,request);
    return this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      this._notificationService.showSuccessMessage()
      return res.objResult
    })
  }
}

