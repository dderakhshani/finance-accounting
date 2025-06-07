import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {AccountReference} from "../../../entities/account-reference";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";


export class GetAccountReferencesByGroupIdQuery extends IRequest<GetAccountReferencesByGroupIdQuery, AccountReference[]> {
  public groupId!: number;

  constructor(pageIndex: number = 0, pageSize: number = 10, searchQueries?: SearchQuery[], orderByProperty?: string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }

  mapFrom(entity: AccountReference[]): GetAccountReferencesByGroupIdQuery {
    return new GetAccountReferencesByGroupIdQuery();
  }

  mapTo(): AccountReference[] {
    return [];
  }

  get url(): string {
    return "/accounting/AccountReference/getallByGroupId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler(GetAccountReferencesByGroupIdQueryHandler.name)
export class GetAccountReferencesByGroupIdQueryHandler implements IRequestHandler<GetAccountReferencesByGroupIdQuery, AccountReference[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetAccountReferencesByGroupIdQuery) {
    let httpRequest = new HttpRequest(request.url, request);

    return await this._httpService.Post<GetAccountReferencesByGroupIdQuery, ServiceResult<PaginatedList<AccountReference>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
