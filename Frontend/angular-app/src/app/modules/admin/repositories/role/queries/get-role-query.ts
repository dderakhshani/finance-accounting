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

export class GetRoleQuery extends IRequest<GetRoleQuery, Role> {
  constructor(public entityId: number) {
    super();
  }
  


  mapFrom(entity: Role): GetRoleQuery {
    throw new ApplicationError(GetRoleQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Role {
    throw new ApplicationError(GetRoleQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/role/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetRoleQueryHandler.name)
export class GetRoleQueryHandler implements IRequestHandler<GetRoleQuery, Role> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetRoleQuery): Promise<Role> {
    let httpRequest: HttpRequest<GetRoleQuery> = new HttpRequest<GetRoleQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;


    return await this._httpService.Get<ServiceResult<Role>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
