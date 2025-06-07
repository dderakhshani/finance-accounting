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
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {WarehouseCountReport} from "../../../entities/warehouse-count-report";

export class GetWarehouseCountReportQuery extends IRequest<GetWarehouseCountReportQuery, any> {
  public warehouseCountFormHeadId?:number
  constructor(public pageIndex: number = 0,
              public pageSize: number = 0,
              public conditions: SearchQuery[] = [], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseCountReport>): GetWarehouseCountReportQuery {
    throw new ApplicationError(GetWarehouseCountReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  mapTo(): PaginatedList<WarehouseCountReport> {
    throw new ApplicationError(GetWarehouseCountReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  get url(): string {
    return "/inventory/WarehouseCountForm/GetWarehouseCountReport";
  }
  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseCountReportQueryHandler.name)
export class GetWarehouseCountReportQueryHandler implements IRequestHandler<GetWarehouseCountReportQuery, PaginatedList<WarehouseCountReport>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseCountReportQuery): Promise<PaginatedList<WarehouseCountReport>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseCountReportQuery> = new HttpRequest<GetWarehouseCountReportQuery>(request.url, request);
    httpRequest.Query += `warehouseCountFormHeadId=${request.warehouseCountFormHeadId}`;

    return await this._httpService.Post<GetWarehouseCountReportQuery, ServiceResult<PaginatedList<WarehouseCountReport>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}

