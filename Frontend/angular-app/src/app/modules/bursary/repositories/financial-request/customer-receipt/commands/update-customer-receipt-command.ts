import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { CreateReceiptCommand } from "./create-receipt-command";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
import { FinancialRequestDetail } from "src/app/modules/bursary/entities/financial-detail";
import { CreateFinancialAttachmentCommand } from "./create-financial-attachment-command";

export class UpdateCustomerReceiptCommand extends IRequest<UpdateCustomerReceiptCommand, FinancialRequest> {

  public id: number | undefined = undefined;
  public documentNo: string | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public voucherHeadId :number | undefined = undefined;
  public voucherHeadCode :number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public description: string | undefined = undefined;
  public financialRequestDetails: CreateReceiptCommand[] = [];
  public financialRequestAttachments : CreateFinancialAttachmentCommand [] = [];

  constructor() {
    super();
  }

  mapFrom(entity: FinancialRequest): UpdateCustomerReceiptCommand {
    this.mapBasics(entity, this)
    this.financialRequestDetails = entity.financialRequestDetails.map((receipt) => {
      return new CreateReceiptCommand().mapFrom(receipt);
    });

    return this;
  }

  mapTo(): FinancialRequest {
    throw new ApplicationError(UpdateCustomerReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/CustomerReceipt/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCustomerReceiptCommandHandler.name)
export class UpdateCustomerReceiptCommandHandler implements IRequestHandler<UpdateCustomerReceiptCommand, FinancialRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCustomerReceiptCommand): Promise<FinancialRequest> {
    let httpRequest: HttpRequest<UpdateCustomerReceiptCommand> = new HttpRequest<UpdateCustomerReceiptCommand>(request.url, request);

    return await this._httpService.Put<UpdateCustomerReceiptCommand, ServiceResult<FinancialRequest>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
