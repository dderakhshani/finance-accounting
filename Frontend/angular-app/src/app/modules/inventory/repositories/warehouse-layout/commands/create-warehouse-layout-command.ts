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
export class CreateWarehouseLayoutCommand extends IRequest<CreateWarehouseLayoutCommand, WarehouseLayout> {

  public id: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public childCount: number | undefined = undefined;
  public lastLevel: boolean | undefined = undefined;
  public entryMode: number | undefined = 0;
  public status: number | undefined = 0;
  public capacity: number | undefined = 50000;
  public isDefault: boolean | undefined = undefined;


  public Items: CreateWarhousteCategoryItemsCommand[] = [];


  constructor() {
    super();
  }


  mapFrom(entity: WarehouseLayout): CreateWarehouseLayoutCommand {
    this.mapBasics(entity, this)
    this.Items = [];

    entity.Items.forEach(x => {

    let mappedItem = new CreateWarhousteCategoryItemsCommand();
      this.Items.push(mappedItem)

    })
    return this;
  }
  mapTo(): WarehouseLayout {
    throw new ApplicationError(CreateWarehouseLayoutCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/WarehouseLayout/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateWarehouseLayoutCommandHandler.name)
export class CreateWarehouseLayoutCommandHandler implements IRequestHandler<CreateWarehouseLayoutCommand, WarehouseLayout> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateWarehouseLayoutCommand): Promise<WarehouseLayout> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateWarehouseLayoutCommand> = new HttpRequest<CreateWarehouseLayoutCommand>(request.url, request);
    request.Items=request.Items.filter(a=>a.commodityCategoryId!=undefined)


    return await this._httpService.Post<CreateWarehouseLayoutCommand, ServiceResult<WarehouseLayout>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
