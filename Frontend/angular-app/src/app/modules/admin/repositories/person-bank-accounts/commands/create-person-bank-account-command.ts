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

export class CreatePersonBankAccountCommand extends IRequest<CreatePersonBankAccountCommand, PersonBankAccount> {

  public personId: number | undefined = undefined;
  public bankId: number | undefined = undefined;
  public bankBranchName: string | undefined = undefined;
  public accountTypeBaseId: number | undefined = undefined;
  public accountNumber: string | undefined = undefined;
  public description: string | undefined = undefined;
  public isDefault: boolean | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: PersonBankAccount): CreatePersonBankAccountCommand {
    throw new ApplicationError(CreatePersonBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonBankAccount {
    throw new ApplicationError(CreatePersonBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personBankAccounts/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePersonBankAccountCommandHandler.name)
export class CreatePersonBankAccountCommandHandler implements IRequestHandler<CreatePersonBankAccountCommand, PersonBankAccount> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreatePersonBankAccountCommand): Promise<PersonBankAccount> {
    let httpRequest: HttpRequest<CreatePersonBankAccountCommand> = new HttpRequest<CreatePersonBankAccountCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreatePersonBankAccountCommand, ServiceResult<PersonBankAccount>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    })

  }
}
