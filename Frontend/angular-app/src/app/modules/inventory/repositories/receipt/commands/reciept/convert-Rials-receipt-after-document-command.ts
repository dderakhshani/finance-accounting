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


export class ConvertToRialsReceiptAfterDocumentCommand extends IRequest<ConvertToRialsReceiptAfterDocumentCommand, Receipt> {

  public editType: number | undefined = undefined;
  public id: number | undefined = undefined;
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
  public isNegative: boolean | undefined = undefined;
  public isFreightChargePaid: boolean | undefined = undefined;

  public receiptDocumentItems: UpdateTemporaryReceiptItemCommand[] = []
  public items: UpdateTemporaryReceiptItemCommand[] = []
  public attachmentIds: number[] = []
  public correctionRequestId: number | undefined = undefined;

  constructor( ) {
    super();
  }

  mapFrom(entity: any): ConvertToRialsReceiptAfterDocumentCommand {

    this.mapBasics(entity, this)
    this.receiptDocumentItems = [];

    if (entity?.items != undefined)
      entity?.items.forEach((x: any) => {

        let mappedItem = new UpdateTemporaryReceiptItemCommand().mapFrom(x)
        this.receiptDocumentItems.push(mappedItem)
      })

    if (entity?.receiptDocumentItems != undefined)
      entity?.receiptDocumentItems.forEach((x: any) => {

        let mappedItem = new UpdateTemporaryReceiptItemCommand().mapFrom(x)
        this.receiptDocumentItems.push(mappedItem)
      })

    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(ConvertToRialsReceiptAfterDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
   
    return "/inventory/Receipt/MechanizedDocumentEditing";
    
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ConvertToRialsReceiptAfterDocumentCommandHandler.name)
export class ConvertToRialsReceiptAfterDocumentCommandHandler implements IRequestHandler<ConvertToRialsReceiptAfterDocumentCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: ConvertToRialsReceiptAfterDocumentCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<ConvertToRialsReceiptAfterDocumentCommand> = new HttpRequest<ConvertToRialsReceiptAfterDocumentCommand>(request.url, request);

   

    return await this._httpService.Post<ConvertToRialsReceiptAfterDocumentCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
