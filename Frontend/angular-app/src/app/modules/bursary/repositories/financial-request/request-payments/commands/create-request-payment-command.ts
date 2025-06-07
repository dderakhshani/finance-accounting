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
import { CreatePaymentCommand } from "./create-payment-command";
import { CreateFinancialAttachmentCommand } from "../../customer-receipt/commands/create-financial-attachment-command";

export class CreateRequestPaymentCommand extends IRequest<CreateRequestPaymentCommand, RequestPayment> {

  public documentNo: string | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public description: string | undefined = undefined;
  public financialRequestPartials: CreatePaymentCommand[] = [];
  public attachments : CreateFinancialAttachmentCommand [] = [];

  constructor() {
    super();
  }

  mapFrom(entity: RequestPayment): CreateRequestPaymentCommand {
    throw new ApplicationError(CreateRequestPaymentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): RequestPayment {
    throw new ApplicationError(CreateRequestPaymentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "bursary/requestpayment/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateRequestPaymentCommandHandler.name)
export class CreateRequestPaymentCommandHandler implements IRequestHandler<CreateRequestPaymentCommand, RequestPayment> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateRequestPaymentCommand): Promise<RequestPayment> {
    let httpRequest: HttpRequest<CreateRequestPaymentCommand> = new HttpRequest<CreateRequestPaymentCommand>(request.url, request);

    return await this._httpService.Post<CreateRequestPaymentCommand, ServiceResult<RequestPayment>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
