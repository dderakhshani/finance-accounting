import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
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
import {WarehouseLayoutTree} from "../../../entities/warehouse-layout";

export class GetWarehouseLayoutTreesQuery extends IRequest<GetWarehouseLayoutTreesQuery, PaginatedList<WarehouseLayoutTree>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseLayoutTree>): GetWarehouseLayoutTreesQuery {
    throw new ApplicationError(GetWarehouseLayoutTreesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseLayoutTree> {
    throw new ApplicationError(GetWarehouseLayoutTreesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayout/GetTreeAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetWarehouseLayoutTreesQueryHandler.name)
export class GetWarehouseLayoutTreesQueryHandler implements IRequestHandler<GetWarehouseLayoutTreesQuery, PaginatedList<WarehouseLayoutTree>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseLayoutTreesQuery): Promise<PaginatedList<WarehouseLayoutTree>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseLayoutTreesQuery> = new HttpRequest<GetWarehouseLayoutTreesQuery>(request.url, request);


    return await this._httpService.Post<GetWarehouseLayoutTreesQuery, ServiceResult<PaginatedList<WarehouseLayoutTree>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
