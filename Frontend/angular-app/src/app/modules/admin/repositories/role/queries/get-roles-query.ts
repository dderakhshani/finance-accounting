import {Role} from "../../../entities/role";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetRolesQuery extends IRequest<GetRolesQuery, PaginatedList<Role>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Role>): GetRolesQuery {
    throw new ApplicationError(GetRolesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Role> {
    throw new ApplicationError(GetRolesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/role/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetRolesQueryHandler.name)
export class GetRolesQueryHandler implements IRequestHandler<GetRolesQuery, PaginatedList<Role>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetRolesQuery): Promise<PaginatedList<Role>> {
    let httpRequest: HttpRequest<GetRolesQuery> = new HttpRequest<GetRolesQuery>(request.url, request);


    return await this._httpService.Post<GetRolesQuery, ServiceResult<PaginatedList<Role>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
