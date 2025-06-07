import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {PayCheque} from "../../../entities/pay-cheque";

export class DeletePayChequeCommand extends IRequest<DeletePayChequeCommand, PayCheque> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: PayCheque): DeletePayChequeCommand {
    throw new ApplicationError(DeletePayChequeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PayCheque {
    throw new ApplicationError(DeletePayChequeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/payCheques/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePayChequeCommandHandler.name)
export class DeletePayChequeCommandHandler implements IRequestHandler<DeletePayChequeCommand, PayCheque> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: DeletePayChequeCommand): Promise<PayCheque> {
    let httpRequest: HttpRequest<DeletePayChequeCommand> = new HttpRequest<DeletePayChequeCommand>(request.url, request);
    httpRequest.Query += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<PayCheque>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
