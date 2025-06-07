import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ServiceResult} from "../../../../../core/models/service-result";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";

export class BulkVoucherStatusUpdateCommand extends IRequest<BulkVoucherStatusUpdateCommand, any>{

  public conditions?:SearchQuery[]
  public status!: number;
  public voucherIds!: number[];
  constructor() {
    super();
  }


  mapFrom(entity: ServiceResult<any>): BulkVoucherStatusUpdateCommand {
    throw Error('not implemented')
  }

  mapTo(): any {
    return undefined;
  }

  get url(): string {
    return "/accounting/vouchersHead/bulkStatusUpdate";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}
@MediatorHandler(BulkVoucherStatusUpdateCommandHandler.name)
export class BulkVoucherStatusUpdateCommandHandler implements IRequestHandler<BulkVoucherStatusUpdateCommand, any>{
  constructor(
    @Inject(HttpService) private _httpService:HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService
  ) {
  }

  async Handle(request: BulkVoucherStatusUpdateCommand){
    let httpRequest = new HttpRequest(request.url,request);
    return this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}

