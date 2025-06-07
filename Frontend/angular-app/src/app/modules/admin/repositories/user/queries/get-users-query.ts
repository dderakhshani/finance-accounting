import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {User} from "../../../entities/user";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetUsersQuery extends IRequest<GetUsersQuery, PaginatedList<User>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<User>): GetUsersQuery {
    throw new ApplicationError(GetUsersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<User> {
    throw new ApplicationError(GetUsersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/User/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetUsersQueryHandler.name)
export class GetUsersQueryHandler implements IRequestHandler<GetUsersQuery, PaginatedList<User>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetUsersQuery): Promise<PaginatedList<User>> {
    let httpRequest: HttpRequest<GetUsersQuery> = new HttpRequest<GetUsersQuery>(request.url, request);

    return await this._httpService.Post<GetUsersQuery, ServiceResult<PaginatedList<User>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
