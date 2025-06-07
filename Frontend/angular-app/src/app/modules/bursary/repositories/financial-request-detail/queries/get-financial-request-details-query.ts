import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {FinancialRequestDetail} from "../../../entities/financial-request-detail";


export class GetFinancialRequestDetailsQuery extends IRequest<GetFinancialRequestDetailsQuery, FinancialRequestDetail[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: FinancialRequestDetail[]): GetFinancialRequestDetailsQuery {
    return new GetFinancialRequestDetailsQuery();
  }

  mapTo(): FinancialRequestDetail[] {
    return [];
  }

  get url(): string {
    return "/bursary/financialRequestDetails/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetFinancialRequestDetailsQueryHandler.name)
export class GetFinancialRequestDetailsQueryHandler implements IRequestHandler<GetFinancialRequestDetailsQuery, FinancialRequestDetail[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetFinancialRequestDetailsQuery) : Promise<FinancialRequestDetail[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<FinancialRequestDetail>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
