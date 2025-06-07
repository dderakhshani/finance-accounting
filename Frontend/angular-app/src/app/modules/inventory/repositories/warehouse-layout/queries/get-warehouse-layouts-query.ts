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
import {WarehouseLayout} from "../../../entities/warehouse-layout";

export class GetWarehouseLayoutsQuery extends IRequest<GetWarehouseLayoutsQuery, PaginatedList<WarehouseLayout>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseLayout>): GetWarehouseLayoutsQuery {
    throw new ApplicationError(GetWarehouseLayoutsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseLayout> {
    throw new ApplicationError(GetWarehouseLayoutsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayout/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseLayoutsQueryHandler.name)
export class GetWarehouseLayoutsQueryHandler implements IRequestHandler<GetWarehouseLayoutsQuery, PaginatedList<WarehouseLayout>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseLayoutsQuery): Promise<PaginatedList<WarehouseLayout>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseLayoutsQuery> = new HttpRequest<GetWarehouseLayoutsQuery>(request.url, request);


    return await this._httpService.Post<GetWarehouseLayoutsQuery, ServiceResult<PaginatedList<WarehouseLayout>>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
