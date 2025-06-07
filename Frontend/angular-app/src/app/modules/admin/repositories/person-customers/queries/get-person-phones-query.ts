import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PersonCustomer} from "../../../entities/person-customer";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetPersonCustomersQuery extends IRequest<GetPersonCustomersQuery, PaginatedList<PersonCustomer>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<PersonCustomer>): GetPersonCustomersQuery {
    throw new ApplicationError(GetPersonCustomersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<PersonCustomer> {
    throw new ApplicationError(GetPersonCustomersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personCustomer/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonCustomersQueryHandler.name)
export class GetPersonCustomersQueryHandler implements IRequestHandler<GetPersonCustomersQuery, PaginatedList<PersonCustomer>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetPersonCustomersQuery): Promise<PaginatedList<PersonCustomer>> {
    let httpRequest: HttpRequest<GetPersonCustomersQuery> = new HttpRequest<GetPersonCustomersQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetPersonCustomersQuery, ServiceResult<PaginatedList<PersonCustomer>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
