import {Role} from "../../../entities/role";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Inject} from "@angular/core";

export class CreateRoleCommand extends IRequest<CreateRoleCommand, Role> {
  public title: string | undefined = undefined;
  public description: string | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public parentId: number | undefined = undefined;
  public permissionsId: number[] = [];
  public cloneFromRole: number | undefined = undefined;



  constructor() {
    super();
  }

  mapFrom(entity: Role): CreateRoleCommand {
    throw new ApplicationError(CreateRoleCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Role {
    throw new ApplicationError(CreateRoleCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/role/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateRoleCommandHandler.name)
export class CreateRoleCommandHandler implements IRequestHandler<CreateRoleCommand, Role> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateRoleCommand): Promise<Role> {
    let httpRequest: HttpRequest<CreateRoleCommand> = new HttpRequest<CreateRoleCommand>(request.url, request);


    return await this._httpService.Post<CreateRoleCommand, ServiceResult<Role>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
