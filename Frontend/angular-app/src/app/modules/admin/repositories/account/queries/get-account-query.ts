import {Inject} from "@angular/core";
import {Account} from "../../../entities/account";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetAccountQuery extends IRequest<GetAccountQuery, Account> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Account): GetAccountQuery {
    throw new ApplicationError(GetAccountQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Account {
    throw new ApplicationError(GetAccountQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/accounts/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountQueryHandler.name)
export class GetAccountQueryHandler implements IRequestHandler<GetAccountQuery, Account> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetAccountQuery): Promise<Account> {
    let httpRequest: HttpRequest<GetAccountQuery> = new HttpRequest<GetAccountQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()


    return await this._httpService.Get<ServiceResult<Account>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
