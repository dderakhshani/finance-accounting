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


export class DeleteFinancialRequestDetailCommand extends IRequest<DeleteFinancialRequestDetailCommand, FinancialRequestDetail> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: FinancialRequestDetail): DeleteFinancialRequestDetailCommand {
    throw new ApplicationError(DeleteFinancialRequestDetailCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): FinancialRequestDetail {
    throw new ApplicationError(DeleteFinancialRequestDetailCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/financialRequestDetails/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteFinancialRequestDetailCommandHandler.name)
export class DeleteFinancialRequestDetailCommandHandler implements IRequestHandler<DeleteFinancialRequestDetailCommand, FinancialRequestDetail> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: DeleteFinancialRequestDetailCommand): Promise<FinancialRequestDetail> {
    let httpRequest: HttpRequest<DeleteFinancialRequestDetailCommand> = new HttpRequest<DeleteFinancialRequestDetailCommand>(request.url, request);
    httpRequest.Query  += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<FinancialRequestDetail>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
