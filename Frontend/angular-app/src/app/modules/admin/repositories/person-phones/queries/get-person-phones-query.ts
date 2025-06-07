import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PersonPhone} from "../../../entities/person-phone";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetPersonPhonesQuery extends IRequest<GetPersonPhonesQuery, PaginatedList<PersonPhone>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<PersonPhone>): GetPersonPhonesQuery {
    throw new ApplicationError(GetPersonPhonesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<PersonPhone> {
    throw new ApplicationError(GetPersonPhonesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personPhones/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonPhonesQueryHandler.name)
export class GetPersonPhonesQueryHandler implements IRequestHandler<GetPersonPhonesQuery, PaginatedList<PersonPhone>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetPersonPhonesQuery): Promise<PaginatedList<PersonPhone>> {
    let httpRequest: HttpRequest<GetPersonPhonesQuery> = new HttpRequest<GetPersonPhonesQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetPersonPhonesQuery, ServiceResult<PaginatedList<PersonPhone>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
