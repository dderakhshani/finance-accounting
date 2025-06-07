import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {Role} from "../../../entities/role";

export class GetMenuItemsRolesQuery extends IRequest<GetMenuItemsRolesQuery, PaginatedList<Role>> {

  constructor(public pageIndex: number = 0,
              public pageSize: number = 0,
              public conditions?: SearchQuery[],
              public orderByProperty: string = '',
              public MenuItemPermissionId?: number) {
    super();
  }

  mapFrom(entity: PaginatedList<Role>): GetMenuItemsRolesQuery {
    throw new ApplicationError(GetMenuItemsRolesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Role> {
    throw new ApplicationError(GetMenuItemsRolesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/MenuItem/GetAllRoleByMenuItemPermissionId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMenuItemsRolesQueryHandler.name)
export class GetMenuItemsRolesQueryHandler implements IRequestHandler<GetMenuItemsRolesQuery, PaginatedList<Role>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetMenuItemsRolesQuery): Promise<PaginatedList<Role>> {
    let httpRequest: HttpRequest<GetMenuItemsRolesQuery> = new HttpRequest<GetMenuItemsRolesQuery>(request.url, request);

    return await this._httpService.Post<GetMenuItemsRolesQuery, ServiceResult<PaginatedList<Role>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
