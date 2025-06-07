
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { UnitCommodityQuota } from "../../../entities/unitCommodityQuota";


export class GetUnitCommodityQuotaQuery extends IRequest<GetUnitCommodityQuotaQuery, UnitCommodityQuota> {


   constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: UnitCommodityQuota): GetUnitCommodityQuotaQuery {
    throw new ApplicationError(GetUnitCommodityQuotaQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): UnitCommodityQuota {
    throw new ApplicationError(GetUnitCommodityQuotaQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "inventory/UnitCommodityQuota/Get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetUnitCommodityQuotaQueryHandler.name)
export class GetUnitCommodityQuotaQueryHandler implements IRequestHandler<GetUnitCommodityQuotaQuery, UnitCommodityQuota> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetUnitCommodityQuotaQuery): Promise<UnitCommodityQuota> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetUnitCommodityQuotaQuery> = new HttpRequest<GetUnitCommodityQuotaQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Get<ServiceResult<UnitCommodityQuota>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
