import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Permission} from "../../../entities/permission";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetPermissionsQuery extends IRequest<GetPermissionsQuery, PaginatedList<Permission>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Permission>): GetPermissionsQuery {
    throw new ApplicationError(GetPermissionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Permission> {
    throw new ApplicationError(GetPermissionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/permission/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPermissionsQueryHandler.name)
export class GetPermissionsQueryHandler implements IRequestHandler<GetPermissionsQuery, PaginatedList<Permission>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPermissionsQuery): Promise<PaginatedList<Permission>> {
    let httpRequest: HttpRequest<GetPermissionsQuery> = new HttpRequest<GetPermissionsQuery>(request.url, request);

    return await this._httpService.Post<GetPermissionsQuery, ServiceResult<PaginatedList<Permission>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
