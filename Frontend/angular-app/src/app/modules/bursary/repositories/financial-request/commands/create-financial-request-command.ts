import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {FinancialRequest} from "../../../entities/financial-request";
import {FinancialRequestDetail} from "../../../entities/financial-request-detail";
import {
  CreateFinancialRequestDetailCommand
} from "../../financial-request-detail/commands/create-financial-request-detail-command";

export class CreateFinancialRequestCommand extends IRequest<CreateFinancialRequestCommand, FinancialRequest> {

  parentId: number | undefined = undefined;
  codeVoucherGroupId: number | undefined = undefined;
  financialRequestDate: Date | undefined = undefined;
  financialStatusBaseId: number | undefined = undefined;
  description: string | undefined = undefined;
  isEmergent: boolean | undefined = undefined;
  isAccumulativePayment: boolean | undefined = undefined;
  financialRequestDetails: CreateFinancialRequestDetailCommand[] = [];


  constructor() {
    super();
  }

  mapFrom(entity: FinancialRequest): CreateFinancialRequestCommand {
    throw new ApplicationError(CreateFinancialRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): FinancialRequest {
    throw new ApplicationError(CreateFinancialRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/financialRequests/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateFinancialRequestCommandHandler.name)
export class CreateFinancialRequestCommandHandler implements IRequestHandler<CreateFinancialRequestCommand, FinancialRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateFinancialRequestCommand): Promise<FinancialRequest> {
    let httpRequest: HttpRequest<CreateFinancialRequestCommand> = new HttpRequest<CreateFinancialRequestCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateFinancialRequestCommand, ServiceResult<FinancialRequest>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
