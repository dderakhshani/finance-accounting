import {Inject} from "@angular/core";
import {Account} from "../../../entities/account";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {UpdatePersonCommand} from "../../person/commands/update-person-command";

export class UpdateAccountCommand extends IRequest<UpdateAccountCommand, Account> {
  public id: number | undefined = undefined;
  public title: string | undefined = undefined;
  public code: string | undefined = undefined;
  public type: number | undefined = undefined;
  public isActive: boolean | undefined = undefined;
  public person: UpdatePersonCommand = new UpdatePersonCommand();
  constructor() {
    super();
  }

  mapFrom(entity: Account): UpdateAccountCommand {
    throw new ApplicationError(UpdateAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Account {
    throw new ApplicationError(UpdateAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/accounts/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAccountCommandHandler.name)
export class UpdateAccountCommandHandler implements IRequestHandler<UpdateAccountCommand, Account> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: UpdateAccountCommand): Promise<Account> {
    let httpRequest: HttpRequest<UpdateAccountCommand> = new HttpRequest<UpdateAccountCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateAccountCommand, ServiceResult<Account>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
