import {ReceiptItem} from "../../../../entities/receipt-item";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";

export class AddDocumentItemsBomQuantityCommand extends IRequest<AddDocumentItemsBomQuantityCommand, ReceiptItem> {

  public documentItemsId: number | undefined = undefined;
  public quantity: number | undefined = undefined;
  public commodityId: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: ReceiptItem): AddDocumentItemsBomQuantityCommand {
    this.mapBasics(entity, this);
    

    return this;
  }

  mapTo(): ReceiptItem {
    throw new ApplicationError(AddDocumentItemsBomQuantityCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/AddDocumentItemsBomQuantity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(AddDocumentItemsBomQuantityCommandHandler.name)
export class AddDocumentItemsBomQuantityCommandHandler implements IRequestHandler<AddDocumentItemsBomQuantityCommand, ReceiptItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddDocumentItemsBomQuantityCommand): Promise<ReceiptItem> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddDocumentItemsBomQuantityCommand> = new HttpRequest<AddDocumentItemsBomQuantityCommand>(request.url, request);
   

    return await this._httpService.Post<AddDocumentItemsBomQuantityCommand, ServiceResult<ReceiptItem>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult
    })

  }
}
