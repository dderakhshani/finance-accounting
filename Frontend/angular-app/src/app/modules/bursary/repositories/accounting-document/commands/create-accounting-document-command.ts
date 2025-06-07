import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { AccountingDocument } from "../../../entities/accounting-document";

export class CreateAccountingDocumentCommand extends IRequest<CreateAccountingDocumentCommand, AccountingDocument> {

  dataList : AccountingDocument[] = []

  constructor() {
    super();
  }

  mapFrom(entity: AccountingDocument): CreateAccountingDocumentCommand {
    throw new ApplicationError(CreateAccountingDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccountingDocument {
    throw new ApplicationError(CreateAccountingDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
   // return "/bursary/CustomerReceipt/AddDocumentForBursaryArticles";
    return "/accounting/VouchersHead/AutoVoucher";

  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateAccountingDocumentCommandHandler.name)
export class CreateAccountingDocumentCommandHandler implements IRequestHandler<CreateAccountingDocumentCommand, AccountingDocument> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateAccountingDocumentCommand): Promise<AccountingDocument> {
    let httpRequest: HttpRequest<CreateAccountingDocumentCommand> = new HttpRequest<CreateAccountingDocumentCommand>(request.url, request);

    return await this._httpService.Post<CreateAccountingDocumentCommand, ServiceResult<AccountingDocument>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
