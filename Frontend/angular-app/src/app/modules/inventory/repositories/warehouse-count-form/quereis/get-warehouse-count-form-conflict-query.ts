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
import {WarehouseCountFormDetail} from "../../../entities/warehouse-count-form-detail";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Warehouse} from "../../../entities/warehouse";
import {PagedList} from "../../../../bursary/entities/pagedList";
import {
  GetWarehousesLastLevelByCodeVoucherGroupIdQuery
} from "../../warehouse/queries/get-warehousesLastLevel-by-codeVoucherGroupId-query ";
import {WarehouseCountFormConflict} from "../../../entities/warehouse-count-form-conflict";
export class GetWarehouseCountFormConflictQuery extends IRequest<GetWarehouseCountFormConflictQuery, any> {
  public warehouseCountFormHeadId?:number
  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions: SearchQuery[] = [], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseCountFormConflict>): GetWarehouseCountFormConflictQuery {
    throw new ApplicationError(GetWarehouseCountFormConflictQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  mapTo(): PaginatedList<WarehouseCountFormConflict> {
    throw new ApplicationError(GetWarehouseCountFormConflictQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  get url(): string {
    return "/inventory/WarehouseCountForm/GetConflictDetails";
  }
  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseCountFormConflictQueryHandler.name)
export class GetWarehouseCountFormConflictQueryHandler implements IRequestHandler<GetWarehouseCountFormConflictQuery, PaginatedList<WarehouseCountFormConflict>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseCountFormConflictQuery): Promise<PaginatedList<WarehouseCountFormConflict>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseCountFormConflictQuery> = new HttpRequest<GetWarehouseCountFormConflictQuery>(request.url, request);
    httpRequest.Query += `warehouseCountFormHeadId=${request.warehouseCountFormHeadId}`;

    return await this._httpService.Post<GetWarehouseCountFormConflictQuery, ServiceResult<PaginatedList<WarehouseCountFormConflict>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
