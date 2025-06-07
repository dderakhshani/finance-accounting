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


export class GetWarehouseStockQuery extends IRequest<GetWarehouseStockQuery, PaginatedList<WarehouseStock>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseStock>): GetWarehouseStockQuery {
    throw new ApplicationError(GetWarehouseStockQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseStock> {
    throw new ApplicationError(GetWarehouseStockQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseStock/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetWarehouseStockQueryHandler.name)
export class GetWarehouseStockQueryHandler implements IRequestHandler<GetWarehouseStockQuery, PaginatedList<WarehouseStock>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseStockQuery): Promise<PaginatedList<WarehouseStock>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseStockQuery> = new HttpRequest<GetWarehouseStockQuery>(request.url, request);


    return await this._httpService.Post<GetWarehouseStockQuery, ServiceResult<PaginatedList<WarehouseStock>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
