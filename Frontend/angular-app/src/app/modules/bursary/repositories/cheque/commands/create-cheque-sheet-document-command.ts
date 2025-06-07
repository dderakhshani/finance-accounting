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

export class CreateChequeSheetDocumentCommand extends IRequest<CreateChequeSheetDocumentCommand, AccountingDocument> {

  dataList : AccountingDocument[] = []
  status !: number

  constructor() {
    super();
  }

  mapFrom(entity: AccountingDocument): CreateChequeSheetDocumentCommand {
    throw new ApplicationError(CreateChequeSheetDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccountingDocument {
    throw new ApplicationError(CreateChequeSheetDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
   // return "/bursary/CustomerReceipt/AddDocumentForBursaryArticles";
    return "/Bursary/ChequeSheet/SetDocumentsForCheque";

  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateChequeSheetDocumentCommandHandler.name)
export class CreateChequeSheetDocumentCommandHandler implements IRequestHandler<CreateChequeSheetDocumentCommand, AccountingDocument> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateChequeSheetDocumentCommand): Promise<AccountingDocument> {
    let httpRequest: HttpRequest<CreateChequeSheetDocumentCommand> = new HttpRequest<CreateChequeSheetDocumentCommand>(request.url, request);

    return await this._httpService.Post<CreateChequeSheetDocumentCommand, ServiceResult<AccountingDocument>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
