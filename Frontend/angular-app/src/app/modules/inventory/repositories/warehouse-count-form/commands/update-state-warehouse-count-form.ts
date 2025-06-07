import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import {WarehouseCountFormDetail} from "../../../entities/warehouse-count-form-detail";
import {UpdateWarehouseCountQuantity} from "../../../entities/update-warehouse-count-quantity";

export class UpdateStateWarehouseCountFormCommand extends IRequest<UpdateStateWarehouseCountFormCommand, any> {
  public id!:number;
  public warehouseStateForm!:number;
  constructor() {
    super();
  }

  mapFrom(entity: ServiceResult<any>): UpdateStateWarehouseCountFormCommand {
    throw Error('not implemented')
  }

  mapTo(): any {
    return undefined;
  }

  get url(): string {
    return "/Inventory/WarehouseCountForm/SetState";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateStateWarehouseCountFormCommandHandler.name)
export class UpdateStateWarehouseCountFormCommandHandler implements IRequestHandler<UpdateStateWarehouseCountFormCommand, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateStateWarehouseCountFormCommand): Promise<any> {
    this._notificationService.isLoader = true;
    let httpRequest = new HttpRequest(request.url, request);
    return this._httpService.Post<any, ServiceResult<any>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}

