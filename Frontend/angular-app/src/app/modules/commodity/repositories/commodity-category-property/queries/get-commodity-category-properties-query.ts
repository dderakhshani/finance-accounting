import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {CommodityCategoryProperty} from "../../../entities/commodity-category-property";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCommodityCategoryPropertiesQuery extends IRequest<GetCommodityCategoryPropertiesQuery, PaginatedList<CommodityCategoryProperty>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<CommodityCategoryProperty>): GetCommodityCategoryPropertiesQuery {
    throw new ApplicationError(GetCommodityCategoryPropertiesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<CommodityCategoryProperty> {
    throw new ApplicationError(GetCommodityCategoryPropertiesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategoryProperty/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoryPropertiesQueryHandler.name)
export class GetCommodityCategoryPropertiesQueryHandler implements IRequestHandler<GetCommodityCategoryPropertiesQuery, PaginatedList<CommodityCategoryProperty>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoryPropertiesQuery): Promise<PaginatedList<CommodityCategoryProperty>> {
    let httpRequest: HttpRequest<GetCommodityCategoryPropertiesQuery> = new HttpRequest<GetCommodityCategoryPropertiesQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetCommodityCategoryPropertiesQuery, ServiceResult<PaginatedList<CommodityCategoryProperty>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
