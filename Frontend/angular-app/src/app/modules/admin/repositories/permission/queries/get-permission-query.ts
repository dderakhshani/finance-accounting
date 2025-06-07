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

export class GetPermissionQuery extends IRequest<GetPermissionQuery, Permission> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Permission): GetPermissionQuery {
    throw new ApplicationError(GetPermissionQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Permission {
    throw new ApplicationError(GetPermissionQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/permission/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPermissionQueryHandler.name)
export class GetPermissionQueryHandler implements IRequestHandler<GetPermissionQuery, Permission> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPermissionQuery): Promise<Permission> {
    let httpRequest: HttpRequest<GetPermissionQuery> = new HttpRequest<GetPermissionQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Permission>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
