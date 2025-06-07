import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";

import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../../core/models/paginated-list";
import { RequestCommodityWarehouse } from "../../../../entities/request-commodity-warehouse";


export class leavingWarehouseRequestByIdQuery extends IRequest<leavingWarehouseRequestByIdQuery, RequestCommodityWarehouse> {

  constructor(public entityId: number, public warehouseId: number,public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '' ) {
    super();
  }

  mapFrom(entity: RequestCommodityWarehouse): leavingWarehouseRequestByIdQuery {
    throw new ApplicationError(leavingWarehouseRequestByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): RequestCommodityWarehouse {
    throw new ApplicationError(leavingWarehouseRequestByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/AraniRequest/leavingWarehouseRequestById";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}



  @MediatorHandler(leavingWarehouseRequestByIdQueryHandler.name)
  export class leavingWarehouseRequestByIdQueryHandler implements IRequestHandler<leavingWarehouseRequestByIdQuery, RequestCommodityWarehouse > {
    constructor(
      @Inject(HttpService) private _httpService: HttpService,
      @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: leavingWarehouseRequestByIdQuery): Promise<RequestCommodityWarehouse> {
      this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<leavingWarehouseRequestByIdQuery> = new HttpRequest<leavingWarehouseRequestByIdQuery>(request.url, request);
      httpRequest.Query += `requestId=${request.entityId}`;
      if (request.warehouseId != undefined) {
        httpRequest.Query += `&warehouseId=${request.warehouseId}`;
      }

      return await this._httpService.Get<ServiceResult<RequestCommodityWarehouse>>(httpRequest).toPromise().then(response => {
        
       
        return response.objResult
      }).finally(() => {
        this._notificationService.isLoader = false;
      })
    }
  }
