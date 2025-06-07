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

export class GetPersonsQuery extends IRequest<GetPersonsQuery, PaginatedList<Person>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions: SearchQuery[] = [], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Person>): GetPersonsQuery {
    throw new ApplicationError(GetPersonsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Person> {
    throw new ApplicationError(GetPersonsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/person/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonsQueryHandler.name)
export class GetPersonsQueryHandler implements IRequestHandler<GetPersonsQuery, PaginatedList<Person>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPersonsQuery): Promise<PaginatedList<Person>> {
    let httpRequest: HttpRequest<GetPersonsQuery> = new HttpRequest<GetPersonsQuery>(request.url, request);

    return await this._httpService.Post<GetPersonsQuery, ServiceResult<PaginatedList<Person>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
