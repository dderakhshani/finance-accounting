import { Inject } from "@angular/core";

import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { UnitCommodityQuota } from "../../../entities/unitCommodityQuota";

export class UpdateUnitCommodityQuotaCommand extends IRequest<UpdateUnitCommodityQuotaCommand, UnitCommodityQuota> {

  public id: number | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public quotaGroupsId: number | undefined = undefined;
  public commodityQuota: number | undefined = undefined;
  public quotaDays: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: UnitCommodityQuota): UpdateUnitCommodityQuotaCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): UnitCommodityQuota {

    throw new ApplicationError(UpdateUnitCommodityQuotaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/UnitCommodityQuota/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateUnitCommodityQuotaCommandHandler.name)
export class UpdateUnitCommodityQuotaCommandHandler implements IRequestHandler<UpdateUnitCommodityQuotaCommand, UnitCommodityQuota> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateUnitCommodityQuotaCommand): Promise<UnitCommodityQuota> {
    this._notificationService.isLoader = true;

    let httpRequest: HttpRequest<UpdateUnitCommodityQuotaCommand> = new HttpRequest<UpdateUnitCommodityQuotaCommand>(request.url, request);




    return await this._httpService.Put<UpdateUnitCommodityQuotaCommand, ServiceResult<UnitCommodityQuota>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
