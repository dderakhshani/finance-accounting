import {RequestPayment} from "../../../../entities/request-payment";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import { FinancialRequestDocumentHeads } from "src/app/modules/bursary/entities/financial-request-document-head";

export class CreateRequestPaymentDocumentHeadCommand extends IRequest<CreateRequestPaymentDocumentHeadCommand, FinancialRequestDocumentHeads> {


  public invoiceBaseId !:number
  public documentHeadsId !:number[]
  public financialRequestId !:number

  constructor() {
    super();
  }

  mapFrom(entity: FinancialRequestDocumentHeads): CreateRequestPaymentDocumentHeadCommand {
    throw new ApplicationError(CreateRequestPaymentDocumentHeadCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): FinancialRequestDocumentHeads {
    throw new ApplicationError(CreateRequestPaymentDocumentHeadCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "bursary/FinancialRequestDocumentHead/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateRequestPaymentDocumentHeadCommandHandler.name)
export class CreateRequestPaymentDocumentHeadCommandHandler implements IRequestHandler<CreateRequestPaymentDocumentHeadCommand, FinancialRequestDocumentHeads> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateRequestPaymentDocumentHeadCommand): Promise<FinancialRequestDocumentHeads> {
    let httpRequest: HttpRequest<CreateRequestPaymentDocumentHeadCommand> = new HttpRequest<CreateRequestPaymentDocumentHeadCommand>(request.url, request);

    return await this._httpService.Post<CreateRequestPaymentDocumentHeadCommand, ServiceResult<FinancialRequestDocumentHeads>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
