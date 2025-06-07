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
import { AddItemsCommand } from "../receipt-items/add-receipt-items-command";


export class AddReceiptCommand extends IRequest<AddReceiptCommand, Receipt> {

  public id: number | undefined = undefined;
  public tags: string | undefined = undefined;
  public parentId: number | undefined = undefined;
  public viewId: number | undefined = undefined;
  public isManual: boolean | undefined = undefined;
  public expireDate: Date | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public requestDate: Date | undefined = undefined;
  public documentNo: string | undefined = undefined;
  public invoiceNo: string | undefined = undefined;
  public requestNo: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public requestNumber: string | undefined = undefined;
  public isImportPurchase: boolean | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public followUpReferenceId: number | undefined = undefined;
  public documentDescription: string | undefined = undefined;
  public requesterReferenceId: number | undefined = undefined;
  public followUpReferenceTitle: string | undefined = undefined;
  public documentStauseBaseValue: number | undefined = undefined;
  public requesterReferenceTitle: string | undefined = undefined;
  public scaleBill: string | undefined = undefined;
  public partNumber: string | undefined = undefined;
  public isDocumentIssuance: boolean | undefined = true;
  //------------------تامین کننده -------------------

  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountHeadId: number | undefined = undefined;


  public receiptDocumentItems: AddItemsCommand[] = []
  public attachmentIds: number[] = []


  public creator: string | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: Receipt): AddReceiptCommand {

    this.mapBasics(entity, this)
    this.receiptDocumentItems = [];
    entity.items.forEach(x => {
      x.quantity = x.remainQuantity;
      let mappedItem = new AddItemsCommand().mapFrom(x)
      this.receiptDocumentItems.push(mappedItem)
    })
    return this;
  }

  mapTo(): Receipt {
    throw new ApplicationError(AddReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/TemporaryReceipt/AddSingleRow";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(AddReceiptCommandHandler.name)
export class AddReceiptCommandHandler implements IRequestHandler<AddReceiptCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddReceiptCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddReceiptCommand> = new HttpRequest<AddReceiptCommand>(request.url, request);
    return await this._httpService.Post<AddReceiptCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
