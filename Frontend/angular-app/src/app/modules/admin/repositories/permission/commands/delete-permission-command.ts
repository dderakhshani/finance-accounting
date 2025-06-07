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

export class DeletePermissionCommand extends IRequest<DeletePermissionCommand, Permission> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Permission): DeletePermissionCommand {
    throw new ApplicationError(DeletePermissionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Permission {
    throw new ApplicationError(DeletePermissionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/permission/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePermissionCommandHandler.name)
export class DeletePermissionCommandHandler implements IRequestHandler<DeletePermissionCommand, Permission> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeletePermissionCommand): Promise<Permission> {
    let httpRequest: HttpRequest<DeletePermissionCommand> = new HttpRequest<DeletePermissionCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<Permission>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
