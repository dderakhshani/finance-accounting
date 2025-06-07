import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {Account} from "../../../entities/account";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { PaginatedList } from "src/app/core/models/paginated-list";

export class GetAccountsQuery extends IRequest<GetAccountsQuery, PaginatedList<Account>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Account>): GetAccountsQuery {
    throw new ApplicationError(GetAccountsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Account> {
    throw new ApplicationError(GetAccountsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/accounts/getAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountsQueryHandler.name)
export class GetAccountsQueryHandler implements IRequestHandler<GetAccountsQuery, PaginatedList<Account>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetAccountsQuery): Promise<PaginatedList<Account>> {
    let httpRequest: HttpRequest<GetAccountsQuery> = new HttpRequest<GetAccountsQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetAccountsQuery, ServiceResult<PaginatedList<Account>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
