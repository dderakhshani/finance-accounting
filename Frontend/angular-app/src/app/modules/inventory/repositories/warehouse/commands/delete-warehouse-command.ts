import {Inject} from "@angular/core";
import { Warehouse } from "../../../entities/warehouse";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteWarehouseCommand extends IRequest<DeleteWarehouseCommand, Warehouse> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Warehouse): DeleteWarehouseCommand {
    throw new ApplicationError(DeleteWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Warehouse {
    throw new ApplicationError(DeleteWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/warehouse/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteWarehouseCommandHandler.name)
export class DeleteWarehouseCommandHandler implements IRequestHandler<DeleteWarehouseCommand, Warehouse> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteWarehouseCommand): Promise<Warehouse> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<DeleteWarehouseCommand> = new HttpRequest<DeleteWarehouseCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Delete<ServiceResult<Warehouse>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
