import {Inject} from "@angular/core";
import {User} from "../../../entities/user";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteUsersByRoleIdCommand extends IRequest<DeleteUsersByRoleIdCommand, any> {

  constructor(public roleId:number,public userIds:number[]) {
    super();
  }

  mapFrom(entity: any): DeleteUsersByRoleIdCommand {
    throw new ApplicationError(DeleteUsersByRoleIdCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(DeleteUsersByRoleIdCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/role/removeUsersFromRole";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteUsersByRoleIdCommandHandler.name)
export class DeleteUsersByRoleIdCommandHandler implements IRequestHandler<DeleteUsersByRoleIdCommand, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteUsersByRoleIdCommand): Promise<any> {
    let httpRequest: HttpRequest<DeleteUsersByRoleIdCommand> = new HttpRequest<DeleteUsersByRoleIdCommand>(request.url, request);

    return await this._httpService.Delete<ServiceResult<any>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
