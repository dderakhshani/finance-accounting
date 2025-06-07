import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {FinancialRequestDetail} from "../../../entities/financial-request-detail";

export class CreateFinancialRequestDetailCommand extends IRequest<CreateFinancialRequestDetailCommand, FinancialRequestDetail> {

  public financialRequestId: number | undefined = undefined;
  public documentTypeBaseId: number | undefined = undefined;
  public creditAccountHeadId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public debitAccountHeadId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public currencyTypeBaseId: number | undefined = undefined;
  public currencyFee: number | undefined = undefined;
  public currencyAmount: number | undefined = undefined;
  public chequeSheetId: number | undefined = undefined;
  public amount: number | undefined = undefined;
  public description: string | undefined = undefined;
  public financialReferenceTypeBaseId: number | undefined = undefined;
  public registrationCode: string | undefined = undefined;
  public paymentCode: string | undefined = undefined;
  public isRial: boolean | undefined = undefined;
  public nonRialStatus: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: FinancialRequestDetail): CreateFinancialRequestDetailCommand {
    this.mapBasics(entity, this)
    return this;
  }

  mapTo(): FinancialRequestDetail {
    throw new ApplicationError(CreateFinancialRequestDetailCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/financialRequestDetails/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateFinancialRequestDetailCommandHandler.name)
export class CreateFinancialRequestDetailCommandHandler implements IRequestHandler<CreateFinancialRequestDetailCommand, FinancialRequestDetail> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateFinancialRequestDetailCommand): Promise<FinancialRequestDetail> {
    let httpRequest: HttpRequest<CreateFinancialRequestDetailCommand> = new HttpRequest<CreateFinancialRequestDetailCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateFinancialRequestDetailCommand, ServiceResult<FinancialRequestDetail>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
