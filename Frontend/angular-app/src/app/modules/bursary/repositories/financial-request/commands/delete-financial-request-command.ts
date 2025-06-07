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

export class DeleteFinancialRequestCommand extends IRequest<DeleteFinancialRequestCommand, FinancialRequest> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: FinancialRequest): DeleteFinancialRequestCommand {
    throw new ApplicationError(DeleteFinancialRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): FinancialRequest {
    throw new ApplicationError(DeleteFinancialRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/financialRequests/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteFinancialRequestCommandHandler.name)
export class DeleteFinancialRequestCommandHandler implements IRequestHandler<DeleteFinancialRequestCommand, FinancialRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: DeleteFinancialRequestCommand): Promise<FinancialRequest> {
    let httpRequest: HttpRequest<DeleteFinancialRequestCommand> = new HttpRequest<DeleteFinancialRequestCommand>(request.url, request);
    httpRequest.Query  += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<FinancialRequest>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
