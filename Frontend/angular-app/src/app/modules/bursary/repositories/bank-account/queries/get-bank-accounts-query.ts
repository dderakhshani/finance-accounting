import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {BankAccount} from "../../../entities/bank-account";

export class GetBankAccountsQuery extends IRequest<GetBankAccountsQuery, BankAccount[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: BankAccount[]): GetBankAccountsQuery {
    return new GetBankAccountsQuery();
  }

  mapTo(): BankAccount[] {
    return [];
  }

  get url(): string {
    return "/bursary/bankAccounts/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetBankAccountsQueryHandler.name)
export class GetBankAccountsQueryHandler implements IRequestHandler<GetBankAccountsQuery, BankAccount[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetBankAccountsQuery) : Promise<BankAccount[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<BankAccount>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
