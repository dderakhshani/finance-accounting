import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {BankBranch} from "../../../entities/bank-branch";

export class GetBankBranchesQuery extends IRequest<GetBankBranchesQuery, BankBranch[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: BankBranch[]): GetBankBranchesQuery {
    return new GetBankBranchesQuery();
  }

  mapTo(): BankBranch[] {
    return [];
  }

  get url(): string {
    return "/bursary/bankBranches/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetBankBranchesQueryHandler.name)
export class GetBankBranchesQueryHandler implements IRequestHandler<GetBankBranchesQuery, BankBranch[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetBankBranchesQuery) : Promise<BankBranch[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<BankBranch>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
