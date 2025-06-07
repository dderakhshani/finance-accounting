import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../../core/models/paginated-list";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {MoadianInvoiceHeader} from "../../../../entities/moadian-invoice-header";

export class GetMoadianInvoiceHeadersQuery extends IRequest<GetMoadianInvoiceHeadersQuery, PaginatedList<MoadianInvoiceHeader>> {

  public isProduction :boolean = false;
  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<MoadianInvoiceHeader>): GetMoadianInvoiceHeadersQuery {
    throw new ApplicationError(GetMoadianInvoiceHeadersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<MoadianInvoiceHeader> {
    throw new ApplicationError(GetMoadianInvoiceHeadersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/moadian/getAllInvoices";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMoadianInvoiceHeadersQueryHandler.name)
export class GetMoadianInvoiceHeadersQueryHandler implements IRequestHandler<GetMoadianInvoiceHeadersQuery, PaginatedList<MoadianInvoiceHeader>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetMoadianInvoiceHeadersQuery): Promise<PaginatedList<MoadianInvoiceHeader>> {
    let httpRequest: HttpRequest<GetMoadianInvoiceHeadersQuery> = new HttpRequest<GetMoadianInvoiceHeadersQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetMoadianInvoiceHeadersQuery, ServiceResult<PaginatedList<MoadianInvoiceHeader>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
