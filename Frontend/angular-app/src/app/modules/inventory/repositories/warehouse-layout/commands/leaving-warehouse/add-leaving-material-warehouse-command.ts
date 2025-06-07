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


export class AddleavingMaterialWarehouseCommand extends IRequest<AddleavingMaterialWarehouseCommand, Receipt> {

  public id: number | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public documentStauseBaseValue: number | undefined = undefined;
  public fromWarehouseId: number | undefined = undefined;
  public toWarehouseId: number | undefined = undefined;
  public documentDescription: string | undefined = undefined;
  public isManual: boolean | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public tags: string | undefined = undefined;
  public documentNo: number | undefined = undefined;
  public viewId: number | undefined = undefined;
  public creator: string | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public debitAccountHeadId: number | undefined = undefined;
  public creditAccountHeadId: number | undefined = undefined;
  public isDocumentIssuance: boolean | undefined = true;
  public receiptDocumentItems: AddItemsCommand[] = []
  constructor() {
    super();
  }

  mapFrom(entity: Receipt): AddleavingMaterialWarehouseCommand {

    this.mapBasics(entity, this)
    this.receiptDocumentItems = [];
    entity.items.forEach(x => {
     
      let mappedItem = new AddItemsCommand().mapFrom(x)
      this.receiptDocumentItems.push(mappedItem)
    })
   
    return this;
  }

  mapTo(): Receipt {
    throw new ApplicationError(AddleavingMaterialWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/LeavingWarehouse/LeavingMaterialWarehouse";
   // return "/inventory/LeavingWarehouse/InsertLeavingWarehouseMaterial";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(AddleavingMaterialWarehouseCommandHandler.name)
export class AddleavingMaterialWarehouseCommandHandler implements IRequestHandler<AddleavingMaterialWarehouseCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddleavingMaterialWarehouseCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddleavingMaterialWarehouseCommand> = new HttpRequest<AddleavingMaterialWarehouseCommand>(request.url, request);


    return await this._httpService.Post<AddleavingMaterialWarehouseCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
