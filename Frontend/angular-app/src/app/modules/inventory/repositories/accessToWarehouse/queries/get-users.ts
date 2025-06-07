
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

import { PaginatedList } from "../../../../../core/models/paginated-list";
import { WarehouseUsers } from "../../../entities/accessToWarehouse";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";


export class GetWarehouseUsersQuery extends IRequest<GetWarehouseUsersQuery, PaginatedList<WarehouseUsers>> {


  constructor(
   
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }
  

  mapFrom(entity: PaginatedList<WarehouseUsers>): GetWarehouseUsersQuery {
    throw new ApplicationError(GetWarehouseUsersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseUsers> {
    throw new ApplicationError(GetWarehouseUsersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/AccessToWarehouse/GetUsers";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseUsersQueryHandler.name)
export class GetWarehouseUsersQueryHandler implements IRequestHandler<GetWarehouseUsersQuery, PaginatedList<WarehouseUsers>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseUsersQuery): Promise<PaginatedList<WarehouseUsers>> {

    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetWarehouseUsersQuery> = new HttpRequest<GetWarehouseUsersQuery>(request.url, request);

    return await this._httpService.Post<GetWarehouseUsersQuery, ServiceResult<PaginatedList<WarehouseUsers>>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
