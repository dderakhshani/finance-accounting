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

export class GetSuggestionWarehouseLayoutByCommodityCategoriesQuery extends IRequest<GetSuggestionWarehouseLayoutByCommodityCategoriesQuery, PaginatedList<WarehouseLayout>> {

  constructor(public entityId: number = 0, public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseLayout>): GetSuggestionWarehouseLayoutByCommodityCategoriesQuery {
    throw new ApplicationError(GetSuggestionWarehouseLayoutByCommodityCategoriesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseLayout> {
    throw new ApplicationError(GetSuggestionWarehouseLayoutByCommodityCategoriesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayout/GetSuggestionWarehouseLayoutByCommodityCategories";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetSuggestionWarehouseLayoutByCommodityCategoriesQueryHandler.name)
export class GetSuggestionWarehouseLayoutByCommodityCategoriesQueryHandler implements IRequestHandler<GetSuggestionWarehouseLayoutByCommodityCategoriesQuery, PaginatedList<WarehouseLayout>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetSuggestionWarehouseLayoutByCommodityCategoriesQuery): Promise<PaginatedList<WarehouseLayout>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetSuggestionWarehouseLayoutByCommodityCategoriesQuery> = new HttpRequest<GetSuggestionWarehouseLayoutByCommodityCategoriesQuery>(request.url, request);
    httpRequest.Query += `commodityId=${request.entityId}`;

    return await this._httpService.Post<GetSuggestionWarehouseLayoutByCommodityCategoriesQuery, ServiceResult<PaginatedList<WarehouseLayout>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
