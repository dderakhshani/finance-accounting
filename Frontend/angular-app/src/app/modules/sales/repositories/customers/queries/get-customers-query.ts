import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Customer} from "../../../entities/Customer";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetCustomersQuery extends IRequest<GetCustomersQuery, PaginatedList<Customer>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Customer>): GetCustomersQuery {
    throw new ApplicationError(GetCustomersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Customer> {
    throw new ApplicationError(GetCustomersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/sale/customers/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCustomersQueryHandler.name)
export class GetCustomersQueryHandler implements IRequestHandler<GetCustomersQuery, PaginatedList<Customer>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetCustomersQuery): Promise<PaginatedList<Customer>> {
    let httpRequest: HttpRequest<GetCustomersQuery> = new HttpRequest<GetCustomersQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetCustomersQuery, ServiceResult<PaginatedList<Customer>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
