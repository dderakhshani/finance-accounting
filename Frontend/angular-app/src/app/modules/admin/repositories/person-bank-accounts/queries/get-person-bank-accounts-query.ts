import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {PersonBankAccount} from "../../../entities/person-bank-account";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetPersonBankAccountsQuery extends IRequest<GetPersonBankAccountsQuery, PaginatedList<PersonBankAccount>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<PersonBankAccount>): GetPersonBankAccountsQuery {
    throw new ApplicationError(GetPersonBankAccountsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<PersonBankAccount> {
    throw new ApplicationError(GetPersonBankAccountsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personBankAccounts/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonBankAccountsQueryHandler.name)
export class GetPersonBankAccountsQueryHandler implements IRequestHandler<GetPersonBankAccountsQuery, PaginatedList<PersonBankAccount>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetPersonBankAccountsQuery): Promise<PaginatedList<PersonBankAccount>> {
    let httpRequest: HttpRequest<GetPersonBankAccountsQuery> = new HttpRequest<GetPersonBankAccountsQuery>(request.url, request);


    return await this._httpService.Post<GetPersonBankAccountsQuery, ServiceResult<PaginatedList<PersonBankAccount>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
