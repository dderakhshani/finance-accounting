import {Inject} from "@angular/core";
import {Receipt} from "../../../../entities/receipt";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import { UpdateTemporaryReceiptItemCommand } from "../receipt-items/update-temporary-receipt-item-command";


export class CreateCorrectionRequestCommand extends IRequest<CreateCorrectionRequestCommand, Receipt> {
  public id: number | undefined = undefined;
  public invoiceNo: string | undefined = undefined;
  public vatDutiesTax: number | undefined = 0;
  public totalProductionCost: number | undefined = 0;
  public totalItemPrice: number | undefined = 0;
  public tags: string | undefined = undefined;
  public discountPercent: number | undefined = 0;
  public extraCost: number | undefined = 0;
  public documentDescription: number | undefined = undefined;
  
  
  public financialOperationNumber: string | undefined = undefined;
  //------------------تامین کننده -------------------
  public debitAccountHeadId: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public creditAccountHeadId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;

  public receiptDocumentItems: UpdateTemporaryReceiptItemCommand[] = []
  public attachmentIds: number[] = []

  constructor() {
    super();
  }

  mapFrom(entity: Receipt): CreateCorrectionRequestCommand {

    this.mapBasics(entity, this)
    this.receiptDocumentItems = [];
    entity.items.forEach(x => {

      let mappedItem = new UpdateTemporaryReceiptItemCommand().mapFrom(x)
      this.receiptDocumentItems.push(mappedItem)
    })
    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(CreateCorrectionRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/CreateCorrectionRequest";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCorrectionRequestCommandHandler.name)
export class CreateCorrectionRequestCommandHandler implements IRequestHandler<CreateCorrectionRequestCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCorrectionRequestCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateCorrectionRequestCommand> = new HttpRequest<CreateCorrectionRequestCommand>(request.url, request);


    return await this._httpService.Post<CreateCorrectionRequestCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
