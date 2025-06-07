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
import { AddItemsProductCommand } from "../../../receipt/commands/receipt-items/add-receipt-items-product-command";



export class LeavingProductCommand extends IRequest<LeavingProductCommand, Receipt> {

 
  public documentDate: Date | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;  
  public documentStauseBaseValue: number | undefined = undefined;

  //------------------تامین کننده -------------------

  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public debitAccountHeadId: number | undefined = undefined;
  public isDocumentIssuance: boolean | undefined = true;
  public receiptDocumentItems: AddItemsProductCommand[] = []
  
  constructor() {
    super();
  }

  mapFrom(entity: any): LeavingProductCommand {

    this.mapBasics(entity, this)
    this.receiptDocumentItems = [];
    entity.items.forEach((x:any) => {
      x.quantity = x.quantity;
     
      let mappedItem = new AddItemsProductCommand().mapFrom(x)
      mappedItem.warehouseId = 1;
      this.receiptDocumentItems.push(mappedItem)
    })
    return this;
  }

  mapTo(): Receipt {
    throw new ApplicationError(LeavingProductCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/LeavingWarehouse/LeaveProductWarehouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(LeavingProductCommandHandler.name)
export class LeavingProductCommandHandler implements IRequestHandler<LeavingProductCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: LeavingProductCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<LeavingProductCommand> = new HttpRequest<LeavingProductCommand>(request.url, request);


    return await this._httpService.Post<LeavingProductCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
