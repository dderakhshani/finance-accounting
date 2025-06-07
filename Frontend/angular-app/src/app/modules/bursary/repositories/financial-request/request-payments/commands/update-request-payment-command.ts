
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../../core/models/service-result";
import { DocumentHead } from "src/app/modules/bursary/entities/document-head";
import { RequestPayment } from "src/app/modules/bursary/entities/request-payment";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { CreatePaymentCommand } from "./create-payment-command";
import { CreateFinancialAttachmentCommand } from "../../customer-receipt/commands/create-financial-attachment-command";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
export class UpdateRequestPaymentCommand extends IRequest<UpdateRequestPaymentCommand, FinancialRequest> {

  public id: number | undefined = undefined;
  public documentNo: string | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public voucherHeadId :number | undefined = undefined;
  public voucherHeadCode :number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public description: string | undefined = undefined;
  public financialRequestPartials: CreatePaymentCommand[] = [];
  public financialRequestAttachments : CreateFinancialAttachmentCommand [] = [];


  constructor() {
    super();
  }

  mapFrom(entity: FinancialRequest): UpdateRequestPaymentCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): FinancialRequest {
    throw new ApplicationError(UpdateRequestPaymentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/financialRequest/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler(UpdateRequestPaymentCommandHandler.name)
export class UpdateRequestPaymentCommandHandler implements IRequestHandler<UpdateRequestPaymentCommand, FinancialRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateRequestPaymentCommand): Promise<FinancialRequest> {
    let httpRequest = new HttpRequest<UpdateRequestPaymentCommand>(request.url, request);




    return await this._httpService.Put<UpdateRequestPaymentCommand, ServiceResult<FinancialRequest>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
