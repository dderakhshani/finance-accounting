import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SaleAgent} from "../../../entities/SaleAgent";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetSaleAgentsQuery extends IRequest<GetSaleAgentsQuery, PaginatedList<SaleAgent>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<SaleAgent>): GetSaleAgentsQuery {
    throw new ApplicationError(GetSaleAgentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<SaleAgent> {
    throw new ApplicationError(GetSaleAgentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/sale/salesAgents/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetSaleAgentsQueryHandler.name)
export class GetSaleAgentsQueryHandler implements IRequestHandler<GetSaleAgentsQuery, PaginatedList<SaleAgent>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetSaleAgentsQuery): Promise<PaginatedList<SaleAgent>> {
    let httpRequest: HttpRequest<GetSaleAgentsQuery> = new HttpRequest<GetSaleAgentsQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetSaleAgentsQuery, ServiceResult<PaginatedList<SaleAgent>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
