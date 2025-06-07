import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../../core/models/paginated-list";
import { WarehouseLayoutsCommodityQuantity } from "../../../../entities/warehouse-layouts-commodity-quantity";


export class GetWarhouseLayoutByCommodityQuantityQuery extends IRequest<GetWarhouseLayoutByCommodityQuantityQuery, PaginatedList<WarehouseLayoutsCommodityQuantity>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseLayoutsCommodityQuantity>): GetWarhouseLayoutByCommodityQuantityQuery {
    throw new ApplicationError(GetWarhouseLayoutByCommodityQuantityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseLayoutsCommodityQuantity> {
    throw new ApplicationError(GetWarhouseLayoutByCommodityQuantityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayoutReport/GetWarhouseLayoutByCommodity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetWarhouseLayoutByCommodityQuantityQueryHandler.name)
export class GetWarhouseLayoutByCommodityQuantityQueryHandler implements IRequestHandler<GetWarhouseLayoutByCommodityQuantityQuery, PaginatedList<WarehouseLayoutsCommodityQuantity>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarhouseLayoutByCommodityQuantityQuery): Promise<PaginatedList<WarehouseLayoutsCommodityQuantity>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarhouseLayoutByCommodityQuantityQuery> = new HttpRequest<GetWarhouseLayoutByCommodityQuantityQuery>(request.url, request);


    return await this._httpService.Post<GetWarhouseLayoutByCommodityQuantityQuery, ServiceResult<PaginatedList<WarehouseLayoutsCommodityQuantity>>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult
    })

  }
}
