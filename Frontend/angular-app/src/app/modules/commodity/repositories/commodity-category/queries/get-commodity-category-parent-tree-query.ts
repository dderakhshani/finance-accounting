import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {CommodityCategory} from "../../../entities/commodity-category";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCommodityCategoryParentTreeQuery extends IRequest<GetCommodityCategoryParentTreeQuery, CommodityCategory[]> {

  constructor(public levelCode: string) {
    super();
  }


  mapFrom(entity: CommodityCategory[]): GetCommodityCategoryParentTreeQuery {
    throw new ApplicationError(GetCommodityCategoryParentTreeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory[] {
    throw new ApplicationError(GetCommodityCategoryParentTreeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategory/GetCategoryParentTree";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoryParentTreeQueryHandler.name)
export class GetCommodityCategoryParentTreeQueryHandler implements IRequestHandler<GetCommodityCategoryParentTreeQuery, CommodityCategory[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoryParentTreeQuery): Promise<CommodityCategory[]> {
    let httpRequest: HttpRequest<GetCommodityCategoryParentTreeQuery> = new HttpRequest<GetCommodityCategoryParentTreeQuery>(request.url, request);
    httpRequest.Query  += `levelCode=${request.levelCode}`;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Get<ServiceResult<CommodityCategory[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
