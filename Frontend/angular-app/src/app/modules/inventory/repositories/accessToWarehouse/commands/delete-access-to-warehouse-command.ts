import {Inject} from "@angular/core";
import { AccessToWarehouse } from "../../../entities/accessToWarehouse";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
export class DeleteAccessToWarehouseCommand extends IRequest<DeleteAccessToWarehouseCommand, AccessToWarehouse> {
  
  constructor(public WarehouseId: number, public UserId: number, public tableName: string) {
    super();
  }

  mapFrom(entity: AccessToWarehouse): DeleteAccessToWarehouseCommand {
    throw new ApplicationError(DeleteAccessToWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccessToWarehouse {
    throw new ApplicationError(DeleteAccessToWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/AccessToWarehouse/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteAccessToWarehouseCommandHandler.name)
export class DeleteAccessToWarehouseCommandHandler implements IRequestHandler<DeleteAccessToWarehouseCommand, AccessToWarehouse> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteAccessToWarehouseCommand): Promise<AccessToWarehouse> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<DeleteAccessToWarehouseCommand> = new HttpRequest<DeleteAccessToWarehouseCommand>(request.url, request);
    
    httpRequest.Query += `WarehouseId=${request.WarehouseId}&UserId=${request.UserId}&TableName=${request.tableName}`;


    return await this._httpService.Delete<ServiceResult<AccessToWarehouse>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
