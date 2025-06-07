import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {WarehouseLayoutProperty} from "../../../../entities/warehouse-layout";

export class DeleteWarehouseLayoutPropertiesCommand extends IRequest<DeleteWarehouseLayoutPropertiesCommand, WarehouseLayoutProperty> {

  constructor(public categoryPropertyId:number,public warehouseLayoutPropertiesId:number) {
    super();
  }

  mapFrom(entity: WarehouseLayoutProperty): DeleteWarehouseLayoutPropertiesCommand {
    throw new ApplicationError(DeleteWarehouseLayoutPropertiesCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): WarehouseLayoutProperty {
    throw new ApplicationError(DeleteWarehouseLayoutPropertiesCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/WarehouseLayoutProperty/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteWarehouseLayoutPropertiesCommandHandler.name)
export class DeleteWarehouseLayoutPropertiesCommandHandler implements IRequestHandler<DeleteWarehouseLayoutPropertiesCommand, WarehouseLayoutProperty> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteWarehouseLayoutPropertiesCommand): Promise<WarehouseLayoutProperty> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<DeleteWarehouseLayoutPropertiesCommand> = new HttpRequest<DeleteWarehouseLayoutPropertiesCommand>(request.url, request);
    httpRequest.Query  += `categoryPropertyId=${request.categoryPropertyId}`;
    httpRequest.Query  += `&warehouseLayoutPropertiesId=${request.warehouseLayoutPropertiesId}`;

    return await this._httpService.Delete<ServiceResult<WarehouseLayoutProperty>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
