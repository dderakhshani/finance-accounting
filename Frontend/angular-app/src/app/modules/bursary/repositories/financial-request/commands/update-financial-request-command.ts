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
import {
  CreateFinancialRequestDetailCommand
} from "../../financial-request-detail/commands/create-financial-request-detail-command";
import {
  UpdateFinancialRequestDetailCommand
} from "../../financial-request-detail/commands/update-financial-request-detail-command";

export class UpdateFinancialRequestCommand extends IRequest<UpdateFinancialRequestCommand, FinancialRequest> {
  id: number | undefined = undefined;
  parentId: number | undefined = undefined;
  codeVoucherGroupId: number | undefined = undefined;
  financialRequestDate: Date | undefined = undefined;
  financialStatusBaseId: number | undefined = undefined;
  description: string | undefined = undefined;
  isEmergent: boolean | undefined = undefined;
  isAccumulativePayment: boolean | undefined = undefined;

  financialRequestDetails: CreateFinancialRequestDetailCommand[] | UpdateFinancialRequestDetailCommand [] = [];
  constructor() {
    super();
  }

  mapFrom(entity: FinancialRequest): UpdateFinancialRequestCommand {
    this.mapBasics(entity, this);
    this.financialRequestDetails = entity.financialRequestDetails.map(x => new UpdateFinancialRequestDetailCommand().mapFrom(x))
    return this;
  }

  mapTo(): FinancialRequest {
    throw new ApplicationError(UpdateFinancialRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/financialRequests/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateFinancialRequestCommandHandler.name)
export class UpdateFinancialRequestCommandHandler implements IRequestHandler<UpdateFinancialRequestCommand, FinancialRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateFinancialRequestCommand): Promise<FinancialRequest> {
    let httpRequest: HttpRequest<UpdateFinancialRequestCommand> = new HttpRequest<UpdateFinancialRequestCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateFinancialRequestCommand, ServiceResult<FinancialRequest>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
