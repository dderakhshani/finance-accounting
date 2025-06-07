import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {BankAccount} from "../../../entities/bank-account";

export class DeleteBankAccountCommand extends IRequest<DeleteBankAccountCommand, BankAccount> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: BankAccount): DeleteBankAccountCommand {
    throw new ApplicationError(DeleteBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BankAccount {
    throw new ApplicationError(DeleteBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/bankAccounts/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteBankAccountCommandHandler.name)
export class DeleteBankAccountCommandHandler implements IRequestHandler<DeleteBankAccountCommand, BankAccount> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: DeleteBankAccountCommand): Promise<BankAccount> {
    let httpRequest: HttpRequest<DeleteBankAccountCommand> = new HttpRequest<DeleteBankAccountCommand>(request.url, request);
    httpRequest.Query  += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<BankAccount>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
