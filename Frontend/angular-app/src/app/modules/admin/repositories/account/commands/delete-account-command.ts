import {Inject} from "@angular/core";
import {Account} from "../../../entities/account";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteAccountCommand extends IRequest<DeleteAccountCommand, Account> {

  constructor() {
    super();
  }

  mapFrom(entity: Account): DeleteAccountCommand {
    throw new ApplicationError(DeleteAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Account {
    throw new ApplicationError(DeleteAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/accounts/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteAccountCommandHandler.name)
export class DeleteAccountCommandHandler implements IRequestHandler<DeleteAccountCommand, Account> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: DeleteAccountCommand): Promise<Account> {
    let httpRequest: HttpRequest<DeleteAccountCommand> = new HttpRequest<DeleteAccountCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Delete<ServiceResult<Account>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
