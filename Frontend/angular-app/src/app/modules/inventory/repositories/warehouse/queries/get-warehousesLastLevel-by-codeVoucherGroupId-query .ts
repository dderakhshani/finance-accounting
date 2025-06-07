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

export class GetWarehousesLastLevelByCodeVoucherGroupIdQuery extends IRequest<GetWarehousesLastLevelByCodeVoucherGroupIdQuery, PaginatedList<Warehouse>> {

  constructor(public CodeVoucherGroupId:number, public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Warehouse>): GetWarehousesLastLevelByCodeVoucherGroupIdQuery {
    throw new ApplicationError(GetWarehousesLastLevelByCodeVoucherGroupIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Warehouse> {
    throw new ApplicationError(GetWarehousesLastLevelByCodeVoucherGroupIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Warehouse/GetWarehousesLastLevelByCodeVoucherGroupId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehousesLastLevelByCodeVoucherGroupIdQueryHandler.name)
export class GetWarehousesLastLevelByCodeVoucherGroupIdQueryHandler implements IRequestHandler<GetWarehousesLastLevelByCodeVoucherGroupIdQuery, PaginatedList<Warehouse>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehousesLastLevelByCodeVoucherGroupIdQuery): Promise<PaginatedList<Warehouse>> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetWarehousesLastLevelByCodeVoucherGroupIdQuery> = new HttpRequest<GetWarehousesLastLevelByCodeVoucherGroupIdQuery>(request.url, request);
    httpRequest.Query += `CodeVoucherGroupId=${request.CodeVoucherGroupId}`;

    return await this._httpService.Post<GetWarehousesLastLevelByCodeVoucherGroupIdQuery, ServiceResult<PaginatedList<Warehouse>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
