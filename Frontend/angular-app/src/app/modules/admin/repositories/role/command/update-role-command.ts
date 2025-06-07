import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Inject} from "@angular/core";
import {Role} from "../../../entities/role";

export class UpdateRoleCommand extends IRequest<UpdateRoleCommand, Role> {
  public id: number | undefined = undefined;
  public title: string | undefined = undefined;
  public description: string | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public parentId: number | undefined = undefined;
  public permissionsId: number[] = [];
  public cloneFromRole: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Role): UpdateRoleCommand {
    this.mapBasics(entity, this);
    this.permissionsId = entity.permissionsId;
    return this;
  }

  mapTo(): Role {
    throw new ApplicationError(UpdateRoleCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/role/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateRoleCommandHandler.name)
export class UpdateRoleCommandHandler implements IRequestHandler<UpdateRoleCommand, Role> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateRoleCommand): Promise<Role> {
    let httpRequest: HttpRequest<UpdateRoleCommand> = new HttpRequest<UpdateRoleCommand>(request.url, request);


    return await this._httpService.Put<UpdateRoleCommand, ServiceResult<Role>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
