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
import { UpdateTemporaryReceiptItemCommand } from "../receipt-items/update-temporary-receipt-item-command";


export class ConvertToRialsReceiptCommand extends IRequest<ConvertToRialsReceiptCommand, Receipt> {
  public id: number | undefined = undefined;
  public editType: number | undefined = undefined;
  public voucherHeadId: number | undefined = undefined;
  public invoiceNo: string | undefined = undefined;
  public vatDutiesTax: number | undefined = 0;
  public totalProductionCost: number | undefined = 0;
  public totalItemPrice: number | undefined = 0;
  public tags: string | undefined = undefined;
  public discountPercent: number | undefined = 0;
  public extraCost: number | undefined = 0;
  public extraCostRialTemp: number | undefined = 0;//نمایشی است ذخیره نشود.
  public extraCostCurrency: number | undefined = 0;//نمایشی است ذخیره نشود.*/
  public documentDescription: number | undefined = undefined;
  public financialOperationNumber: string | undefined = undefined;
  public invoiceDate: Date | undefined = undefined;
  public documentId: Date | undefined = undefined;
  //------------------تامین کننده -------------------
  public debitAccountHeadId: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public creditAccountHeadId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public extraCostAccountHeadId: number | undefined = undefined;
  public extraCostAccountReferenceGroupId: number | undefined = undefined;
  public extraCostAccountReferenceId: number | undefined = undefined;
  public scaleBill: string | undefined = undefined;
  public isNegative: boolean | undefined = undefined;
  public isFreightChargePaid: boolean | undefined = undefined;

  public receiptDocumentItems: UpdateTemporaryReceiptItemCommand[] = []
  public attachmentIds: number[] = []


  constructor() {
    super();
  }

  mapFrom(entity: any): ConvertToRialsReceiptCommand {

    this.mapBasics(entity, this)
    this.receiptDocumentItems = [];

    if (entity?.items != undefined)
      entity?.items.forEach((x: any) => {

        let mappedItem = new UpdateTemporaryReceiptItemCommand().mapFrom(x)
        mappedItem.warehouseId = entity.warehouseId
        this.receiptDocumentItems.push(mappedItem)
        
      })

    if (entity?.receiptDocumentItems != undefined)
      entity?.receiptDocumentItems.forEach((x: any) => {

        let mappedItem = new UpdateTemporaryReceiptItemCommand().mapFrom(x)
        mappedItem.warehouseId = entity.warehouseId
        this.receiptDocumentItems.push(mappedItem)
      })

    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(ConvertToRialsReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {


    
    if (this.voucherHeadId == undefined || this.voucherHeadId==0) {
      return "/inventory/Receipt/ConvertToRialsReceipt";
    }
    else {
      return "/inventory/Receipt/CreateCorrectionRequest";
    }

  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ConvertToRialsReceiptCommandHandler.name)
export class ConvertToRialsReceiptCommandHandler implements IRequestHandler<ConvertToRialsReceiptCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: ConvertToRialsReceiptCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<ConvertToRialsReceiptCommand> = new HttpRequest<ConvertToRialsReceiptCommand>(request.url, request);


    return await this._httpService.Post<ConvertToRialsReceiptCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
