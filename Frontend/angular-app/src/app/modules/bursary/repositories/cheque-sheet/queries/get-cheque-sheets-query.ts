import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {ChequeSheet} from "../../../entities/cheque-sheet";


export class GetChequeSheetsQuery extends IRequest<GetChequeSheetsQuery, ChequeSheet[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: ChequeSheet[]): GetChequeSheetsQuery {
    return new GetChequeSheetsQuery();
  }

  mapTo(): ChequeSheet[] {
    return [];
  }

  get url(): string {
    return "/bursary/chequeSheets/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetChequeSheetsQueryHandler.name)
export class GetChequeSheetsQueryHandler implements IRequestHandler<GetChequeSheetsQuery, ChequeSheet[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetChequeSheetsQuery) : Promise<ChequeSheet[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<ChequeSheet>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
