import {Inject} from "@angular/core";
import { AccessToWarehouse } from "../../../entities/accessToWarehouse";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreateAccessToWarehouseCommand extends IRequest<CreateAccessToWarehouseCommand, AccessToWarehouse> {



  public warehouseId: number | undefined = undefined;
  public userId: number | undefined = undefined;
  public tableName: string | undefined = undefined;
  public description: string | undefined = undefined;
 
  constructor() {
    super();
  }

  mapFrom(entity: AccessToWarehouse): CreateAccessToWarehouseCommand {
    throw new ApplicationError(CreateAccessToWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccessToWarehouse {
    throw new ApplicationError(CreateAccessToWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/AccessToWarehouse/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateAccessToWarehouseCommandHandler.name)
export class CreateAccessToWarehouseCommandHandler implements IRequestHandler<CreateAccessToWarehouseCommand, AccessToWarehouse> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateAccessToWarehouseCommand): Promise<AccessToWarehouse> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateAccessToWarehouseCommand> = new HttpRequest<CreateAccessToWarehouseCommand>(request.url, request);


    return await this._httpService.Post<CreateAccessToWarehouseCommand, ServiceResult<AccessToWarehouse>>(httpRequest).toPromise().then(response => {
      

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
