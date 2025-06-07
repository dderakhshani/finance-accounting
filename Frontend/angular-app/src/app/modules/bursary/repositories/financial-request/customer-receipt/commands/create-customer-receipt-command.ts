import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import { CreateReceiptCommand } from "./create-receipt-command";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
import { CreateFinancialAttachmentCommand } from "./create-financial-attachment-command";


export class CreateCustomerReceiptCommand extends IRequest<CreateCustomerReceiptCommand, FinancialRequest> {


  public documentNo: string | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public description: string | undefined = undefined;
  public isBursaryDocument : boolean | undefined = undefined;
  public repeatNo : number | undefined = undefined;
  
  public financialRequestDetails: CreateReceiptCommand[] = [];
  public attachments : CreateFinancialAttachmentCommand [] = [];

  constructor() {
    super();
  }

  mapFrom(entity: FinancialRequest): CreateCustomerReceiptCommand {
    throw new ApplicationError(CreateCustomerReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): FinancialRequest {
    throw new ApplicationError(CreateCustomerReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/CustomerReceipt/Add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCustomerReceiptCommandHandler.name)
export class CreateCustomerReceiptCommandHandler implements IRequestHandler<CreateCustomerReceiptCommand, FinancialRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCustomerReceiptCommand): Promise<FinancialRequest> {
    let httpRequest: HttpRequest<CreateCustomerReceiptCommand> = new HttpRequest<CreateCustomerReceiptCommand>(request.url, request);

    return await this._httpService.Post<CreateCustomerReceiptCommand, ServiceResult<FinancialRequest>>(httpRequest).toPromise().then(response => {

      if (response.succeed){
      this._notificationService.showSuccessMessage();
      return response.objResult;
    }
    else
    return response.objResult;

    })

  }
}
