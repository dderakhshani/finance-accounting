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


export class UpdateFinancialRequestDetailCommand extends IRequest<UpdateFinancialRequestDetailCommand, FinancialRequestDetail> {
  public id: number | undefined = undefined;
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

  mapFrom(entity: FinancialRequestDetail): UpdateFinancialRequestDetailCommand {
    this.mapBasics(entity, this);
    return this;
  }

  mapTo(): FinancialRequestDetail {
    throw new ApplicationError(UpdateFinancialRequestDetailCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/financialRequestDetails/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateFinancialRequestDetailCommandHandler.name)
export class UpdateFinancialRequestDetailCommandHandler implements IRequestHandler<UpdateFinancialRequestDetailCommand, FinancialRequestDetail> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateFinancialRequestDetailCommand): Promise<FinancialRequestDetail> {
    let httpRequest: HttpRequest<UpdateFinancialRequestDetailCommand> = new HttpRequest<UpdateFinancialRequestDetailCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateFinancialRequestDetailCommand, ServiceResult<FinancialRequestDetail>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
