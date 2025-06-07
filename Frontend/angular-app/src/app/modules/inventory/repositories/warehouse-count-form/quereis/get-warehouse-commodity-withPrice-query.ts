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
import {warehouseCommodityWithPrice} from "../../../entities/warehouse-commodity-withPrice";

export class GetWarehouseCommodityWithPriceQuery extends IRequest<GetWarehouseCommodityWithPriceQuery, any> {
  public warehouseId?:number
  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions: SearchQuery[] = [], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<warehouseCommodityWithPrice>): GetWarehouseCommodityWithPriceQuery {
    throw new ApplicationError(GetWarehouseCommodityWithPriceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  mapTo(): PaginatedList<warehouseCommodityWithPrice> {
    throw new ApplicationError(GetWarehouseCommodityWithPriceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  get url(): string {
    return "/inventory/WarehouseCountForm/GetWarehouseCommoditiesWithPrice";
  }
  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseCommodityWithPriceQueryHandler.name)
export class GetWarehouseCommodityWithPriceQueryHandler implements IRequestHandler<GetWarehouseCommodityWithPriceQuery, PaginatedList<warehouseCommodityWithPrice>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseCommodityWithPriceQuery): Promise<PaginatedList<warehouseCommodityWithPrice>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseCommodityWithPriceQuery> = new HttpRequest<GetWarehouseCommodityWithPriceQuery>(request.url, request);
    httpRequest.Query += `warehouseId=${request.warehouseId}`;

    return await this._httpService.Post<GetWarehouseCommodityWithPriceQuery, ServiceResult<PaginatedList<warehouseCommodityWithPrice>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
