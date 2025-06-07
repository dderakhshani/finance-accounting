import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Person} from "../../../entities/person";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class SearchPersonQuery extends IRequest<SearchPersonQuery, Person[]> {

  constructor(public searchTerm:string) {
    super();
  }


  mapFrom(entity: Person[]): SearchPersonQuery {
    throw new ApplicationError(SearchPersonQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Person[] {
    throw new ApplicationError(SearchPersonQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/person/searchPerson";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(SearchPersonQueryHandler.name)
export class SearchPersonQueryHandler implements IRequestHandler<SearchPersonQuery, Person[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: SearchPersonQuery): Promise<Person[]> {
    let httpRequest: HttpRequest<SearchPersonQuery> = new HttpRequest<SearchPersonQuery>(request.url, request);
    httpRequest.Query  += `search=${request.searchTerm}`;

    return await this._httpService.Post<SearchPersonQuery, ServiceResult<PaginatedList<Person>>>(httpRequest).toPromise().then(response => {
      return response.objResult.data
    })
  }
}
