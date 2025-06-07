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

export class GetAccountHeadQuery extends IRequest<GetAccountHeadQuery, AccountHead> {

  constructor(public entityId?: number) {
    super();
  }
  mapFrom(entity: AccountHead): GetAccountHeadQuery {
    return new GetAccountHeadQuery();
  }

  mapTo(): AccountHead {
    return new AccountHead();
  }

  get url(): string {
    return "/accounting/AccountHead/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetAccountHeadQueryHandler.name)
export class GetAccountHeadQueryHandler implements IRequestHandler<GetAccountHeadQuery, AccountHead>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetAccountHeadQuery) : Promise<AccountHead> {
    let httpRequest = new HttpRequest(request.url,request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Post<any,ServiceResult<AccountHead>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
