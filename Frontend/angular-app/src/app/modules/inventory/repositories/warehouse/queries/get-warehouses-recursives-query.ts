import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { Warehouse } from "../../../entities/warehouse";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetWarehousesLastLevelQuery extends IRequest<GetWarehousesLastLevelQuery, PaginatedList<Warehouse>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Warehouse>): GetWarehousesLastLevelQuery {
    throw new ApplicationError(GetWarehousesLastLevelQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Warehouse> {
    throw new ApplicationError(GetWarehousesLastLevelQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Warehouse/GetWarehousesLastLevel";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehousesLastLevelQueryHandler.name)
export class GetWarehousesLastLevelQueryHandler implements IRequestHandler<GetWarehousesLastLevelQuery, PaginatedList<Warehouse>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehousesLastLevelQuery): Promise<PaginatedList<Warehouse>> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetWarehousesLastLevelQuery> = new HttpRequest<GetWarehousesLastLevelQuery>(request.url, request);


    return await this._httpService.Post<GetWarehousesLastLevelQuery, ServiceResult<PaginatedList<Warehouse>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
