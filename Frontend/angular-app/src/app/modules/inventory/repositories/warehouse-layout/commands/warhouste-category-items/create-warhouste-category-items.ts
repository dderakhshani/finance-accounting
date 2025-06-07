import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { WarehouseLayout } from "../../../../entities/warehouse-layout";
import { WarhousteCategoryItems } from "../../../../entities/warehouse-layout";
export class CreateWarhousteCategoryItemsCommand extends IRequest<CreateWarhousteCategoryItemsCommand, WarhousteCategoryItems> {

  public warehouseLayoutId: number | undefined = undefined;
  public commodityCategoryId: number | undefined = undefined;
  public categoryPropertyId: number | undefined = undefined;
  public categoryPropertyItemId: number | undefined = undefined;
  public warehouseLayoutCategorysId: number | undefined = undefined;
  public warehouseLayoutPropertiesId: number | undefined = undefined;




  constructor() {
    super();
  }
  mapFrom(entity: WarhousteCategoryItems): CreateWarhousteCategoryItemsCommand {

    this.mapBasics(entity, this);

    return this;
  }


  mapTo(): WarhousteCategoryItems {
    throw new ApplicationError(CreateWarhousteCategoryItemsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/WarehouseLayout/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateWarehouseLayoutCommandHandler.name)
export class CreateWarehouseLayoutCommandHandler implements IRequestHandler<CreateWarhousteCategoryItemsCommand, WarhousteCategoryItems> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateWarhousteCategoryItemsCommand): Promise<WarhousteCategoryItems> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateWarhousteCategoryItemsCommand> = new HttpRequest<CreateWarhousteCategoryItemsCommand>(request.url, request);


    return await this._httpService.Post<CreateWarhousteCategoryItemsCommand, ServiceResult<WarhousteCategoryItems>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
