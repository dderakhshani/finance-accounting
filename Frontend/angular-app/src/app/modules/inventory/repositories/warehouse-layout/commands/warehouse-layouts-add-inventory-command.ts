import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { WarehouseLayout } from "../../../entities/warehouse-layout";

export class WarehouseLayoutsAddInventoryCommand extends IRequest<WarehouseLayoutsAddInventoryCommand, WarehouseLayout> {

  public commodityTitle: string | undefined = undefined;
  public commodityCode: string | undefined = undefined

  public layoutTitle: string | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public quantity: number | undefined = undefined;
  public warehouseLayoutId: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  //------------------تامین کننده -------------------
  public debitAccountHeadId: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public creditAccountHeadId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;


constructor() {
  super();
}


mapFrom(entity: WarehouseLayout): WarehouseLayoutsAddInventoryCommand {
  this.mapBasics(entity, this)



  return this;
}
mapTo(): WarehouseLayout {
  throw new ApplicationError(WarehouseLayoutsAddInventoryCommand.name, this.mapTo.name, 'Not Implemented Yet')
}

  get url(): string {
  return "/Inventory/WarehouseOperations/AddWarehouseInventory";
}

  get validationRules(): ValidationRule[] {
  return [];
}
}

@MediatorHandler(WarehouseLayoutsAddInventoryCommandHandler.name)
export class WarehouseLayoutsAddInventoryCommandHandler implements IRequestHandler<WarehouseLayoutsAddInventoryCommand, WarehouseLayout> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: WarehouseLayoutsAddInventoryCommand): Promise<WarehouseLayout> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<WarehouseLayoutsAddInventoryCommand> = new HttpRequest<WarehouseLayoutsAddInventoryCommand>(request.url, request);



    return await this._httpService.Post<WarehouseLayoutsAddInventoryCommand, ServiceResult<WarehouseLayout>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
