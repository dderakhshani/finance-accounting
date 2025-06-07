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
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {Commodity} from "../../../entities/commodity";

export class GetCommoditiesQuery extends IRequest<GetCommoditiesQuery, PaginatedList<Commodity>> {

  constructor(public commodityCategoryId :number,public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Commodity>): GetCommoditiesQuery {
    throw new ApplicationError(GetCommoditiesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Commodity> {
    throw new ApplicationError(GetCommoditiesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodity/getAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommoditiesQueryHandler.name)
export class GetCommoditiesQueryHandler implements IRequestHandler<GetCommoditiesQuery, PaginatedList<Commodity>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommoditiesQuery): Promise<PaginatedList<Commodity>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCommoditiesQuery> = new HttpRequest<GetCommoditiesQuery>(request.url, request);
    httpRequest.Query += `CommodityCategoryId=${request.commodityCategoryId}`;

    return await this._httpService.Post<GetCommoditiesQuery, ServiceResult<PaginatedList<Commodity>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
