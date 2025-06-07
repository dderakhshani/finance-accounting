import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {PersonBankAccount} from "../../../entities/person-bank-account";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetPersonBankAccountQuery extends IRequest<GetPersonBankAccountQuery, PersonBankAccount> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: PersonBankAccount): GetPersonBankAccountQuery {
    throw new ApplicationError(GetPersonBankAccountQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonBankAccount {
    throw new ApplicationError(GetPersonBankAccountQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personBankAccounts/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonBankAccountQueryHandler.name)
export class GetPersonBankAccountQueryHandler implements IRequestHandler<GetPersonBankAccountQuery, PersonBankAccount> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetPersonBankAccountQuery): Promise<PersonBankAccount> {
    let httpRequest: HttpRequest<GetPersonBankAccountQuery> = new HttpRequest<GetPersonBankAccountQuery>(request.url, request);

    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<PersonBankAccount>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
