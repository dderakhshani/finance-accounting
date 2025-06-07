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

import { AddItemsCommand } from "../receipt-items/add-receipt-items-command";


export class AddGeroupTemporaryReceiptCommand extends IRequest<AddGeroupTemporaryReceiptCommand, Receipt> {

  public id: number | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public documentDescription: string | undefined = undefined;
  public isManual: boolean | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public requestNumber: number | undefined = undefined;
  public tags: string | undefined = undefined;
  public partNumber:string | undefined = undefined;
  public receiptDocumentItems: AddItemsCommand[] = []
  public isDocumentIssuance: boolean | undefined = true;

  public creator:string | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: Receipt): AddGeroupTemporaryReceiptCommand {
    this.mapBasics(entity, this)
    this.receiptDocumentItems = [];
    entity.items.forEach(x => {

      let mappedItem = new AddItemsCommand().mapFrom(x)
      this.receiptDocumentItems.push(mappedItem)
    })
    return this;
  }

  mapTo(): Receipt {
    throw new ApplicationError(AddGeroupTemporaryReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/TemporaryReceipt/Add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(AddGeroupTemporaryReceiptCommandHandler.name)
export class AddGeroupTemporaryReceiptCommandHandler implements IRequestHandler<AddGeroupTemporaryReceiptCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddGeroupTemporaryReceiptCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddGeroupTemporaryReceiptCommand> = new HttpRequest<AddGeroupTemporaryReceiptCommand>(request.url, request);


    return await this._httpService.Post<AddGeroupTemporaryReceiptCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {


      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
