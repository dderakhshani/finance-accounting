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
import { CreateWarhousteCategoryItemsCommand } from "./warhouste-category-items/create-warhouste-category-items";

export class UpdateWarehouseLayoutCommand extends IRequest<UpdateWarehouseLayoutCommand, WarehouseLayout> {


  public id: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public capacity: number | undefined = undefined;
  public entryMode: number | undefined = undefined;
  public lastLevel: boolean | undefined = undefined;
  public isDefault: boolean | undefined = undefined;
  public status: number | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public Items: CreateWarhousteCategoryItemsCommand[] = [];

  constructor() {
    super();
  }

  mapFrom(entity: WarehouseLayout): UpdateWarehouseLayoutCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): WarehouseLayout {

    throw new ApplicationError(UpdateWarehouseLayoutCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/WarehouseLayout/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateWarehouseLayoutCommandHandler.name)
export class UpdateWarehouseLayoutCommandHandler implements IRequestHandler<UpdateWarehouseLayoutCommand, WarehouseLayout> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateWarehouseLayoutCommand): Promise<WarehouseLayout> {
    this._notificationService.isLoader = true;

    let httpRequest: HttpRequest<UpdateWarehouseLayoutCommand> = new HttpRequest<UpdateWarehouseLayoutCommand>(request.url, request);
    return await this._httpService.Put<UpdateWarehouseLayoutCommand, ServiceResult<WarehouseLayout>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
