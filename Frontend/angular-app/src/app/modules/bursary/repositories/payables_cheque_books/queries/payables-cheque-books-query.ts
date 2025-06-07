import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {ChequeBook} from "../../../../bursary/entities/cheque-book";

import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class PayablesChequeBooksQuery extends IRequest<PayablesChequeBooksQuery, ChequeBook[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: ChequeBook[]): PayablesChequeBooksQuery {
    return new PayablesChequeBooksQuery();
  }

  mapTo(): ChequeBook[] {
    return [];
  }

  get url(): string {
    return "/bursary/Payables_ChequeBooks/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(PayablesChequeBooksQueryHandler.name)
export class PayablesChequeBooksQueryHandler implements IRequestHandler<PayablesChequeBooksQuery, ChequeBook[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:PayablesChequeBooksQuery) : Promise<ChequeBook[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<ChequeBook>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
