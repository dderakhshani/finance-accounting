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

export class UpdateDocumentItemsBomQuantityCommand extends IRequest<UpdateDocumentItemsBomQuantityCommand, ReceiptItem> {
  public id: number | undefined = undefined;
  public quantity: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: ReceiptItem): UpdateDocumentItemsBomQuantityCommand {
    this.mapBasics(entity, this);
    

    return this;
  }

  mapTo(): ReceiptItem {
    throw new ApplicationError(UpdateDocumentItemsBomQuantityCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateDocumentItemsBomQuantity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateDocumentItemsBomQuantityCommandHandler.name)
export class UpdateDocumentItemsBomQuantityCommandHandler implements IRequestHandler<UpdateDocumentItemsBomQuantityCommand, ReceiptItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateDocumentItemsBomQuantityCommand): Promise<ReceiptItem> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateDocumentItemsBomQuantityCommand> = new HttpRequest<UpdateDocumentItemsBomQuantityCommand>(request.url, request);
   

    return await this._httpService.Post<UpdateDocumentItemsBomQuantityCommand, ServiceResult<ReceiptItem>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult
    })

  }
}
