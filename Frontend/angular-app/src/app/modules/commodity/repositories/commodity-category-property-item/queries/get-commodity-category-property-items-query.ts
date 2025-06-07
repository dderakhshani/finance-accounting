import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {CommodityCategoryPropertyItem} from "../../../entities/commodity-category-property-item";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCommodityCategoryPropertyItemsQuery extends IRequest<GetCommodityCategoryPropertyItemsQuery, PaginatedList<CommodityCategoryPropertyItem>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<CommodityCategoryPropertyItem>): GetCommodityCategoryPropertyItemsQuery {
    throw new ApplicationError(GetCommodityCategoryPropertyItemsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<CommodityCategoryPropertyItem> {
    throw new ApplicationError(GetCommodityCategoryPropertyItemsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/CommodityCategoryPropertyItem/getAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoryPropertyItemsQueryHandler.name)
export class GetCommodityCategoryPropertyItemsQueryHandler implements IRequestHandler<GetCommodityCategoryPropertyItemsQuery, PaginatedList<CommodityCategoryPropertyItem>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoryPropertyItemsQuery): Promise<PaginatedList<CommodityCategoryPropertyItem>> {
    let httpRequest: HttpRequest<GetCommodityCategoryPropertyItemsQuery> = new HttpRequest<GetCommodityCategoryPropertyItemsQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetCommodityCategoryPropertyItemsQuery, ServiceResult<PaginatedList<CommodityCategoryPropertyItem>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
