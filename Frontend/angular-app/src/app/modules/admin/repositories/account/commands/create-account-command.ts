import {ServiceResult} from "src/app/core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {Account} from "../../../entities/account";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";

export class CreateAccountCommand extends IRequest<CreateAccountCommand, Account> {

  public title: string | undefined = undefined;
  public code: string | undefined = undefined;
  public type: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Account): CreateAccountCommand {
    throw new ApplicationError(CreateAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Account {
    throw new ApplicationError(CreateAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/accounts/create";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateAccountCommandHandler.name)
export class CreateAccountCommandHandler implements IRequestHandler<CreateAccountCommand, Account> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: CreateAccountCommand): Promise<Account> {
    let httpRequest: HttpRequest<CreateAccountCommand> = new HttpRequest<CreateAccountCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateAccountCommand, ServiceResult<Account>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
