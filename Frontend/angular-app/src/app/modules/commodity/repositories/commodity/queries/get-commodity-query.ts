import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {Commodity} from "../../../entities/commodity";

export class GetCommodityQuery extends IRequest<GetCommodityQuery, Commodity> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Commodity): GetCommodityQuery {
    throw new ApplicationError(GetCommodityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Commodity {
    throw new ApplicationError(GetCommodityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodity/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityQueryHandler.name)
export class GetCommodityQueryHandler implements IRequestHandler<GetCommodityQuery, Commodity> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityQuery): Promise<Commodity> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCommodityQuery> = new HttpRequest<GetCommodityQuery>(request.url, request);
    httpRequest.Query += `id=${request.entityId}`;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Get<ServiceResult<Commodity>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
