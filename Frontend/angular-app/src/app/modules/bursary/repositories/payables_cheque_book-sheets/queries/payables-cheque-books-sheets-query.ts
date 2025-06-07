import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {ChequeBooksSheet} from "../../../entities/cheque-books-sheet";

export class PayablesChequeBooksSheetsQuery extends IRequest<PayablesChequeBooksSheetsQuery, ChequeBooksSheet[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: ChequeBooksSheet[]): PayablesChequeBooksSheetsQuery {
    return new PayablesChequeBooksSheetsQuery();
  }

  mapTo(): ChequeBooksSheet[] {
    return [];
  }

  get url(): string {
    return "/bursary/Payables_ChequeBookSheets/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(PayablesChequeBooksSheetsQueryHandler.name)
export class PayablesChequeBooksSheetsQueryHandler implements IRequestHandler<PayablesChequeBooksSheetsQuery, ChequeBooksSheet[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:PayablesChequeBooksSheetsQuery) : Promise<ChequeBooksSheet[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<ChequeBooksSheet>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
