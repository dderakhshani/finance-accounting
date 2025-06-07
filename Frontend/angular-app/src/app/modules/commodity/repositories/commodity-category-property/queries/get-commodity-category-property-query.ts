import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {CommodityCategoryProperty} from "../../../entities/commodity-category-property";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCommodityCategoryPropertyQuery extends IRequest<GetCommodityCategoryPropertyQuery, CommodityCategoryProperty> {
  constructor(public entityId: number) {
    super();
  }

  mapFrom(entity: CommodityCategoryProperty): GetCommodityCategoryPropertyQuery {
    throw new ApplicationError(GetCommodityCategoryPropertyQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategoryProperty {
    throw new ApplicationError(GetCommodityCategoryPropertyQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategoryProperty";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoryPropertyQueryHandler.name)
export class GetCommodityCategoryPropertyQueryHandler implements IRequestHandler<GetCommodityCategoryPropertyQuery, CommodityCategoryProperty> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoryPropertyQuery): Promise<CommodityCategoryProperty> {
    let httpRequest: HttpRequest<GetCommodityCategoryPropertyQuery> = new HttpRequest<GetCommodityCategoryPropertyQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Get<ServiceResult<CommodityCategoryProperty>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
