import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {PayCheque} from "../../../entities/pay-cheque";


export class GetPayChequesQuery extends IRequest<GetPayChequesQuery, PayCheque[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: PayCheque[]): GetPayChequesQuery {
    return new GetPayChequesQuery();
  }

  mapTo(): PayCheque[] {
    return [];
  }

  get url(): string {
    return "/bursary/payCheques/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetPayChequesQueryHandler.name)
export class GetPayChequesQueryHandler implements IRequestHandler<GetPayChequesQuery, PayCheque[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetPayChequesQuery) : Promise<PayCheque[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<PayCheque>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
