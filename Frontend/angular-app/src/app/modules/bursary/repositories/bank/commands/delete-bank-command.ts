import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Bank} from "../../../entities/bank";

export class DeleteBankCommand extends IRequest<DeleteBankCommand, Bank> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: Bank): DeleteBankCommand {
    throw new ApplicationError(DeleteBankCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Bank {
    throw new ApplicationError(DeleteBankCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/banks/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteBankCommandHandler.name)
export class DeleteBankCommandHandler implements IRequestHandler<DeleteBankCommand, Bank> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: DeleteBankCommand): Promise<Bank> {
    let httpRequest: HttpRequest<DeleteBankCommand> = new HttpRequest<DeleteBankCommand>(request.url, request);
    httpRequest.Query  += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<Bank>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
