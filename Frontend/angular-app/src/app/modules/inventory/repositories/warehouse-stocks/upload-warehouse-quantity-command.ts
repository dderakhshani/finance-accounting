import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../core/models/paginated-list";
import { WarehouseStock } from "../../entities/warehouse-stock";
import { Warehouse } from "../../entities/warehouse";


export class UpdateWarehouseStockQuantityQuery extends IRequest<UpdateWarehouseStockQuantityQuery, any> {


  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Warehouse): UpdateWarehouseStockQuantityQuery {
    throw new ApplicationError(UpdateWarehouseStockQuantityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Warehouse {
    throw new ApplicationError(UpdateWarehouseStockQuantityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseStock/GetExcel";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateWarehouseStockQuantityQueryHandler.name)
export class UpdateWarehouseStockQuantityQueryHandler implements IRequestHandler<UpdateWarehouseStockQuantityQuery, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateWarehouseStockQuantityQuery): Promise<any> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateWarehouseStockQuantityQuery> = new HttpRequest<UpdateWarehouseStockQuantityQuery>(request.url, request);
    httpRequest.Query += `id=${request.entityId}`;



    return await this._httpService.Get<ServiceResult<any>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
