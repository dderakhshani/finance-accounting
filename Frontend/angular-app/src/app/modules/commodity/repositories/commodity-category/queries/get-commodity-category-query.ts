import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { CommodityCategory } from "../../../entities/commodity-category";


export class GetCommodityCategoryQuery extends IRequest<GetCommodityCategoryQuery, CommodityCategory> {


   constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: CommodityCategory): GetCommodityCategoryQuery {
    throw new ApplicationError(GetCommodityCategoryQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory {
    throw new ApplicationError(GetCommodityCategoryQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/CommodityCategory/Get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoryQueryHandler.name)
export class GetCommodityCategoryQueryHandler implements IRequestHandler<GetCommodityCategoryQuery, CommodityCategory> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoryQuery): Promise<CommodityCategory> {
    let httpRequest: HttpRequest<GetCommodityCategoryQuery> = new HttpRequest<GetCommodityCategoryQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Get<ServiceResult<CommodityCategory>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
