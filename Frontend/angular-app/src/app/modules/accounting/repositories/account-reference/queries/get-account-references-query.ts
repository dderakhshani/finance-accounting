import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {AccountReference} from "../../../entities/account-reference";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {ApplicationError} from "../../../../../core/exceptions/application-error";

export class GetAccountReferencesQuery extends IRequest<GetAccountReferencesQuery,PaginatedList<AccountReference>> {
  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: PaginatedList<AccountReference>): GetAccountReferencesQuery {
    return new GetAccountReferencesQuery();
  }

  mapTo(): PaginatedList<AccountReference> {
    throw new ApplicationError(GetAccountReferencesQuery.name,this.mapTo.name,'not implemented yet')
  }

  get url(): string {
    return "/accounting/AccountReference/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler(GetAccountReferencesQueryHandler.name)
export class GetAccountReferencesQueryHandler implements IRequestHandler<GetAccountReferencesQuery,PaginatedList<AccountReference>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {}
  async Handle(request:GetAccountReferencesQuery){
    let httpRequest = new HttpRequest(request.url,request);

    return await this._httpService.Post<GetAccountReferencesQuery,ServiceResult<PaginatedList<AccountReference>>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
