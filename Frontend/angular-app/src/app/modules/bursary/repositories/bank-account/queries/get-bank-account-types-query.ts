import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {BankAccountTypes} from "../../../entities/bank-account-type";

export class GetBankAccountTypesQuery extends IRequest<GetBankAccountTypesQuery, BankAccountTypes[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: BankAccountTypes[]): GetBankAccountTypesQuery {
    return new GetBankAccountTypesQuery();
  }

  mapTo(): BankAccountTypes[] {
    return [];
  }

  get url(): string {
    return "/bursary/bankAccounts/getBankAccountTypesList";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler( GetBankAccountTypesQueryHandler.name)
export class GetBankAccountTypesQueryHandler implements IRequestHandler<GetBankAccountTypesQuery, BankAccountTypes[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetBankAccountTypesQuery) : Promise<BankAccountTypes[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<BankAccountTypes>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
