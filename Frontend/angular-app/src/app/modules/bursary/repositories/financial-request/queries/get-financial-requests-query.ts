import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {FinancialRequest} from "../../../entities/financial-request";
import {ApplicationError} from "../../../../../core/exceptions/application-error";

export class GetFinancialRequestsQuery extends IRequest<GetFinancialRequestsQuery, PaginatedList<FinancialRequest>> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: PaginatedList<FinancialRequest>): GetFinancialRequestsQuery {
    throw new ApplicationError(GetFinancialRequestsQuery.name,this.mapTo.name,'Not Implemented Yet')

  }

  mapTo(): PaginatedList<FinancialRequest> {
    throw new ApplicationError(GetFinancialRequestsQuery.name,this.mapTo.name,'Not Implemented Yet')

  }

  get url(): string {
    return "/bursary/financialRequests/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetFinancialRequestsQueryHandler.name)
export class GetFinancialRequestsQueryHandler implements IRequestHandler<GetFinancialRequestsQuery, PaginatedList<FinancialRequest>>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetFinancialRequestsQuery) : Promise<PaginatedList<FinancialRequest>> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<FinancialRequest>>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
