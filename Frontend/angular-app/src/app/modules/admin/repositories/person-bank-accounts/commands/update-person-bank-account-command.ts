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

export class UpdatePersonBankAccountCommand extends IRequest<UpdatePersonBankAccountCommand, PersonBankAccount> {
  public id: number | undefined = undefined;
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

  mapFrom(entity: PersonBankAccount): UpdatePersonBankAccountCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): PersonBankAccount {
    throw new ApplicationError(UpdatePersonBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personBankAccounts/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePersonBankAccountCommandHandler.name)
export class UpdatePersonBankAccountCommandHandler implements IRequestHandler<UpdatePersonBankAccountCommand, PersonBankAccount> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: UpdatePersonBankAccountCommand): Promise<PersonBankAccount> {
    let httpRequest: HttpRequest<UpdatePersonBankAccountCommand> = new HttpRequest<UpdatePersonBankAccountCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()


    return await this._httpService.Put<UpdatePersonBankAccountCommand, ServiceResult<PersonBankAccount>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
