import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {PayableDocument} from "../../../entities/payableDocument";


export class GetPayableDocuments extends IRequest<GetPayableDocuments, PayableDocument[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: PayableDocument[]): GetPayableDocuments {
    return new GetPayableDocuments();
  }

  mapTo(): PayableDocument[] {
    return [];
  }

  get url(): string {
    return "/bursary/Payables_ChequeBookSheets/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(PayablesPayableDocumentsQueryHandler.name)
export class PayablesPayableDocumentsQueryHandler implements IRequestHandler<GetPayableDocuments, PayableDocument[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetPayableDocuments) : Promise<PayableDocument[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<PaginatedList<PayableDocument>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
