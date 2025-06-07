import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {ServiceResult} from "../../../../../core/models/service-result";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class LockVoucherForUpdateCommand extends IRequest<LockVoucherForUpdateCommand, any>{

  constructor(public voucherId: number) {
    super();
  }


  mapFrom(entity: ServiceResult<any>): LockVoucherForUpdateCommand {
    throw Error('not implemented')
  }

  mapTo(): any {
    return undefined;
  }

  get url(): string {
    return "/accounting/vouchersHead/lockVoucherForUpdate";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}
@MediatorHandler(LockVoucherForUpdateHandler.name)
export class LockVoucherForUpdateHandler implements IRequestHandler<LockVoucherForUpdateCommand, any>{
  constructor(
    @Inject(HttpService) private _httpService:HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService
  ) {
  }

  async Handle(request: LockVoucherForUpdateCommand){
    let httpRequest = new HttpRequest(request.url,request);
    return this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}

