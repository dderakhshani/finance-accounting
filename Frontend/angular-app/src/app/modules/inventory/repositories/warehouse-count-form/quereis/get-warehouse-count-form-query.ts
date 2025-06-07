import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {WarehouseCountFormHead} from "../../../entities/warehouse-count-form-head";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
export class GetWarehouseCountFormQuery extends IRequest<GetWarehouseCountFormQuery, any> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions: SearchQuery[] = [], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: WarehouseCountFormHead): GetWarehouseCountFormQuery {
    throw new ApplicationError(GetWarehouseCountFormQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  mapTo(): WarehouseCountFormHead {
    throw new ApplicationError(GetWarehouseCountFormQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  get url(): string {
    return "/inventory/WarehouseCountForm/GetAll";
  }
  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseCountFormQueryHandler.name)
export class GetWarehouseCountFormQueryHandler implements IRequestHandler<GetWarehouseCountFormQuery, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseCountFormQuery): Promise<any> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseCountFormQuery> = new HttpRequest<GetWarehouseCountFormQuery>(request.url, request);
    return await this._httpService.Post<GetWarehouseCountFormQuery, ServiceResult<PaginatedList<WarehouseCountFormHead>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}

