import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {AccountHead} from "../../../entities/account-head";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {DataCacheService} from "../../../../../core/services/data/data-cache.service";

export class GetAccountHeadsQuery extends IRequest<GetAccountHeadsQuery, AccountHead[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: AccountHead[]): GetAccountHeadsQuery {
    return new GetAccountHeadsQuery();
  }

  mapTo(): AccountHead[] {
    return [];
  }

  get url(): string {
    return "/accounting/AccountHead/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetAccountHeadsQueryHandler.name)
export class GetAccountHeadsQueryHandler implements IRequestHandler<GetAccountHeadsQuery, AccountHead[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetAccountHeadsQuery) : Promise<AccountHead[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<AccountHead>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
