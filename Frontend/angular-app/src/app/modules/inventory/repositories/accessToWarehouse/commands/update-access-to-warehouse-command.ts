import { Inject } from "@angular/core";
import { AccessToWarehouse } from "../../../entities/accessToWarehouse";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";

export class UpdateAccessToWarehouseCommand extends IRequest<UpdateAccessToWarehouseCommand, AccessToWarehouse> {


  public id: number | undefined = undefined;
  public AccessToWarehouseId: number | undefined = undefined;
  public userId: string | undefined = undefined;
  public tableName: boolean | undefined = true;
  public description: number | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: AccessToWarehouse): UpdateAccessToWarehouseCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): AccessToWarehouse {

    throw new ApplicationError(UpdateAccessToWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/AccessToWarehouse/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAccessToWarehouseCommandHandler.name)
export class UpdateAccessToWarehouseCommandHandler implements IRequestHandler<UpdateAccessToWarehouseCommand, AccessToWarehouse> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateAccessToWarehouseCommand): Promise<AccessToWarehouse> {
    this._notificationService.isLoader = true;

    let httpRequest: HttpRequest<UpdateAccessToWarehouseCommand> = new HttpRequest<UpdateAccessToWarehouseCommand>(request.url, request);




    return await this._httpService.Put<UpdateAccessToWarehouseCommand, ServiceResult<AccessToWarehouse>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
