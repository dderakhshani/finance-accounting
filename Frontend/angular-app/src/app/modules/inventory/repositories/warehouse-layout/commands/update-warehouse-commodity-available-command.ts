import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {WarehouseLayout} from "../../../entities/warehouse-layout";

export class UpdateWarehouseCommodityAvailableCommand extends IRequest<UpdateWarehouseCommodityAvailableCommand, WarehouseLayout> {

  public warehouseId: number | undefined = undefined;
  public yearId: number | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: WarehouseLayout): UpdateWarehouseCommodityAvailableCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): WarehouseLayout {

    throw new ApplicationError(UpdateWarehouseCommodityAvailableCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/WarehouseOperations/UpdateWarehouseCommodityAvailable";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateWarehouseCommodityAvailableCommandHandler.name)
export class UpdateWarehouseCommodityAvailableCommandHandler implements IRequestHandler<UpdateWarehouseCommodityAvailableCommand, WarehouseLayout> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateWarehouseCommodityAvailableCommand): Promise<WarehouseLayout> {
    this._notificationService.isLoader = true;

    let httpRequest: HttpRequest<UpdateWarehouseCommodityAvailableCommand> = new HttpRequest<UpdateWarehouseCommodityAvailableCommand>(request.url, request);
    return await this._httpService.Post<UpdateWarehouseCommodityAvailableCommand, ServiceResult<WarehouseLayout>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
