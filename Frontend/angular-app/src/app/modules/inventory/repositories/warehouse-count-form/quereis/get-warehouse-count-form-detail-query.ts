import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {WarehouseCountFormDetail} from "../../../entities/warehouse-count-form-detail";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
export class GetWarehouseCountFormDetailQuery extends IRequest<GetWarehouseCountFormDetailQuery, any> {
  public warehouseCountFormHeadId!:number;
  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions: SearchQuery[] = [], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseCountFormDetail>): GetWarehouseCountFormDetailQuery {
    throw new ApplicationError(GetWarehouseCountFormDetailQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  mapTo(): PaginatedList<WarehouseCountFormDetail> {
    throw new ApplicationError(GetWarehouseCountFormDetailQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  get url(): string {
    return "/inventory/WarehouseCountForm/GetDetailsByHeadId";
  }
  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseCountFormDetailQueryHandler.name)
export class GetWarehouseCountFormDetailQueryHandler implements IRequestHandler<GetWarehouseCountFormDetailQuery, PaginatedList<WarehouseCountFormDetail>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseCountFormDetailQuery): Promise<PaginatedList<WarehouseCountFormDetail>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseCountFormDetailQuery> = new HttpRequest<GetWarehouseCountFormDetailQuery>(request.url, request);
    httpRequest.Query += `warehouseCountFormHeadId=${request.warehouseCountFormHeadId}`;

    return await this._httpService.Post<GetWarehouseCountFormDetailQuery, ServiceResult<PaginatedList<WarehouseCountFormDetail>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
