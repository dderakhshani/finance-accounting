import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {CommodityCategoryPropertyItem} from "../../../entities/commodity-category-property-item";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCommodityCategoryPropertyItemQuery extends IRequest<GetCommodityCategoryPropertyItemQuery, CommodityCategoryPropertyItem> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: CommodityCategoryPropertyItem): GetCommodityCategoryPropertyItemQuery {
    throw new ApplicationError(GetCommodityCategoryPropertyItemQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategoryPropertyItem {
    throw new ApplicationError(GetCommodityCategoryPropertyItemQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategoryPropertyItem/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoryPropertyItemQueryHandler.name)
export class GetCommodityCategoryPropertyItemQueryHandler implements IRequestHandler<GetCommodityCategoryPropertyItemQuery, CommodityCategoryPropertyItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoryPropertyItemQuery): Promise<CommodityCategoryPropertyItem> {
    let httpRequest: HttpRequest<GetCommodityCategoryPropertyItemQuery> = new HttpRequest<GetCommodityCategoryPropertyItemQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Get<ServiceResult<CommodityCategoryPropertyItem>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
