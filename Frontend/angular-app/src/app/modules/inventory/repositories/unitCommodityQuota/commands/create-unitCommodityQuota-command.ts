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


export class CreateUnitCommodityQuotaCommand extends IRequest<CreateUnitCommodityQuotaCommand, UnitCommodityQuota> {

  public commodityId: number | undefined = undefined;
  public quotaGroupsId: number | undefined = undefined;
  public commodityQuota: number | undefined = undefined;
  public quotaDays: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: UnitCommodityQuota): CreateUnitCommodityQuotaCommand {
    throw new ApplicationError(CreateUnitCommodityQuotaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): UnitCommodityQuota {
    throw new ApplicationError(CreateUnitCommodityQuotaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/UnitCommodityQuota/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateUnitCommodityQuotaCommandHandler.name)
export class CreateUnitCommodityQuotaCommandHandler implements IRequestHandler<CreateUnitCommodityQuotaCommand, UnitCommodityQuota> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateUnitCommodityQuotaCommand): Promise<UnitCommodityQuota> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateUnitCommodityQuotaCommand> = new HttpRequest<CreateUnitCommodityQuotaCommand>(request.url, request);


    return await this._httpService.Post<CreateUnitCommodityQuotaCommand, ServiceResult<UnitCommodityQuota>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
