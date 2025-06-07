import { Inject } from "@angular/core";
import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../../core/models/paginated-list";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { WarehouseLayoutsCommodityQuantity } from "src/app/modules/inventory/entities/warehouse-layouts-commodity-quantity";

export class GetWarehouseLayoutQuantitiesQuery extends IRequest<GetWarehouseLayoutQuantitiesQuery, PaginatedList<WarehouseLayoutsCommodityQuantity>> {

  constructor(
    public warehouseId: number = 0,
    public CommodityId: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseLayoutsCommodityQuantity>): GetWarehouseLayoutQuantitiesQuery {
    throw new ApplicationError(GetWarehouseLayoutQuantitiesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseLayoutsCommodityQuantity> {
    throw new ApplicationError(GetWarehouseLayoutQuantitiesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayoutReport/GetWarehouseLayoutQuantities";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetWarehouseLayoutQuantitiesQueryHandler.name)
export class GetWarehouseLayoutQuantitiesQueryHandler implements IRequestHandler<GetWarehouseLayoutQuantitiesQuery, PaginatedList<WarehouseLayoutsCommodityQuantity>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseLayoutQuantitiesQuery): Promise<PaginatedList<WarehouseLayoutsCommodityQuantity>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseLayoutQuantitiesQuery> = new HttpRequest<GetWarehouseLayoutQuantitiesQuery>(request.url, request);

    httpRequest.Query += `warehouseId=${request.warehouseId}&CommodityId=${request.CommodityId}`;
    return await this._httpService.Post<GetWarehouseLayoutQuantitiesQuery, ServiceResult<PaginatedList<WarehouseLayoutsCommodityQuantity>>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult
    })

  }
}
