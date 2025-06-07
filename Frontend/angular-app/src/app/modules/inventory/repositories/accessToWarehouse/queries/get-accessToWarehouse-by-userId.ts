
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { AccessToWarehouse } from "../../../entities/accessToWarehouse";


export class GetAccessToWarehouseByUserIdQuery extends IRequest<GetAccessToWarehouseByUserIdQuery, number[]> {


  constructor(public UserId: number, public tableName:string) {
    super();
  }
  

  mapFrom(entity: number[]): GetAccessToWarehouseByUserIdQuery {
    throw new ApplicationError(GetAccessToWarehouseByUserIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): number[] {
    throw new ApplicationError(GetAccessToWarehouseByUserIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/AccessToWarehouse/GetUserId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccessToWarehouseByUserIdQueryHandler.name)
export class GetAccessToWarehouseByUserIdQueryHandler implements IRequestHandler<GetAccessToWarehouseByUserIdQuery, number[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAccessToWarehouseByUserIdQuery): Promise<number[]> {
    let httpRequest: HttpRequest<GetAccessToWarehouseByUserIdQuery> = new HttpRequest<GetAccessToWarehouseByUserIdQuery>(request.url, request);
    httpRequest.Query += `UserId=${request.UserId}&TableName=${request.tableName}`;



    return await this._httpService.Get<ServiceResult<number[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
