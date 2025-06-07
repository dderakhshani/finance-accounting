import {HttpRequest} from "../../../../../core/services/http/http-request";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {PersonBankAccount} from "../../../entities/person-bank-account";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class DeletePersonBankAccountCommand extends IRequest<DeletePersonBankAccountCommand, PersonBankAccount> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: PersonBankAccount): DeletePersonBankAccountCommand {
    throw new ApplicationError(DeletePersonBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonBankAccount {
    throw new ApplicationError(DeletePersonBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personBankAccounts/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePersonBankAccountCommandHandler.name)
export class DeletePersonBankAccountCommandHandler implements IRequestHandler<DeletePersonBankAccountCommand, PersonBankAccount> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: DeletePersonBankAccountCommand): Promise<PersonBankAccount> {
    let httpRequest: HttpRequest<DeletePersonBankAccountCommand> = new HttpRequest<DeletePersonBankAccountCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<PersonBankAccount>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    })
  }
}
