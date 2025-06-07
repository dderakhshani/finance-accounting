import { Inject } from "@angular/core";
import { Receipt } from "../../../../entities/receipt";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { AddItemsCommand } from "../../../receipt/commands/receipt-items/add-receipt-items-command";



export class UpdateLeavingMaterialWarehouseCommand extends IRequest<UpdateLeavingMaterialWarehouseCommand, Receipt> {

  public id: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public documentDescription: string | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public tags: string | undefined = undefined;
  public documentNo: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public debitAccountHeadId: number | undefined = undefined;
  public creditAccountHeadId: number | undefined = undefined;
  public receiptDocumentItems: AddItemsCommand[] = [];
  public isDocumentIssuance: boolean | undefined = true;



  constructor() {
    super();
  }

  mapFrom(entity: Receipt): UpdateLeavingMaterialWarehouseCommand {

    this.mapBasics(entity, this)
    this.receiptDocumentItems = [];
    entity.items.forEach(x => {

      let mappedItem = new AddItemsCommand().mapFrom(x)
      this.receiptDocumentItems.push(mappedItem)
    })
    return this;
  }

  mapTo(): Receipt {
    throw new ApplicationError(UpdateLeavingMaterialWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/LeavingWarehouse/UpdateLeavingMaterialWarehouse";
    //return "/inventory/LeavingWarehouse/UpdateLeavingWarehouseMaterial";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(UpdateLeavingMaterialWarehouseCommandHandler.name)
export class UpdateLeavingMaterialWarehouseCommandHandler implements IRequestHandler<UpdateLeavingMaterialWarehouseCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateLeavingMaterialWarehouseCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateLeavingMaterialWarehouseCommand> = new HttpRequest<UpdateLeavingMaterialWarehouseCommand>(request.url, request);


    return await this._httpService.Post<UpdateLeavingMaterialWarehouseCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
