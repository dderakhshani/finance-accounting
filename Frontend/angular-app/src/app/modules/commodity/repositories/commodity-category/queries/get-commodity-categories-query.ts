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
import {CommodityCategory} from "../../../entities/commodity-category";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetCommodityCategoriesQuery extends IRequest<GetCommodityCategoriesQuery, CommodityCategory[]> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CommodityCategory[]): GetCommodityCategoriesQuery {
    throw new ApplicationError(GetCommodityCategoriesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory[] {
    throw new ApplicationError(GetCommodityCategoriesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/CommodityCategory/getAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoriesQueryHandler.name)
export class GetCommodityCategoriesQueryHandler implements IRequestHandler<GetCommodityCategoriesQuery, CommodityCategory[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoriesQuery): Promise<CommodityCategory[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCommodityCategoriesQuery> = new HttpRequest<GetCommodityCategoriesQuery>(request.url, request);

    if (!request.orderByProperty) request.orderByProperty = "Id ASC"

    return await this._httpService.Post<GetCommodityCategoriesQuery, ServiceResult<PaginatedList<CommodityCategory>>>(httpRequest).toPromise().then(response => {
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
