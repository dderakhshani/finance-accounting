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
export class DeleteUnitCommodityQuotaCommand extends IRequest<DeleteUnitCommodityQuotaCommand, UnitCommodityQuota> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: UnitCommodityQuota): DeleteUnitCommodityQuotaCommand {
    throw new ApplicationError(DeleteUnitCommodityQuotaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): UnitCommodityQuota {
    throw new ApplicationError(DeleteUnitCommodityQuotaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/UnitCommodityQuota/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteUnitCommodityQuotaCommandHandler.name)
export class DeleteUnitCommodityQuotaCommandHandler implements IRequestHandler<DeleteUnitCommodityQuotaCommand, UnitCommodityQuota> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteUnitCommodityQuotaCommand): Promise<UnitCommodityQuota> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<DeleteUnitCommodityQuotaCommand> = new HttpRequest<DeleteUnitCommodityQuotaCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Delete<ServiceResult<UnitCommodityQuota>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
