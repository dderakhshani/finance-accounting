import {Role} from "../../../entities/role";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";

export class DeleteRoleCommand extends IRequest<DeleteRoleCommand, Role> {

  constructor( public entityId:number ) {
    super();
  }

  mapFrom(entity: Role): DeleteRoleCommand {
    throw new ApplicationError(DeleteRoleCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Role {
    throw new ApplicationError(DeleteRoleCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/role/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteRoleCommandHandler.name)
export class DeleteRoleCommandHandler implements IRequestHandler<DeleteRoleCommand, Role> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteRoleCommand): Promise<Role> {
    let httpRequest: HttpRequest<DeleteRoleCommand> = new HttpRequest<DeleteRoleCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}` ;


    return await this._httpService.Delete<ServiceResult<Role>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
