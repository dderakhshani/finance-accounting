import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { Permission } from "../../../entities/permission";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { TermsOfAccess } from "./create-permission-command";

export class UpdatePermissionCommand extends IRequest<UpdatePermissionCommand, Permission> {

  public id: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public title: string | undefined = undefined;
  public isDataRowLimiter: boolean | undefined = undefined;
  public subSystem: string | undefined = undefined;

  public tableName: string | undefined = undefined;
  public termsOfAccesses: TermsOfAccess[] | undefined = undefined;
  public accessToAll: boolean | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: Permission): UpdatePermissionCommand {
    this.mapBasics(entity, this);
    return this;
  }

  mapTo(): Permission {
    throw new ApplicationError(UpdatePermissionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/permission/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePermissionCommandHandler.name)
export class UpdatePermissionCommandHandler implements IRequestHandler<UpdatePermissionCommand, Permission> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdatePermissionCommand): Promise<Permission> {
    let httpRequest: HttpRequest<UpdatePermissionCommand> = new HttpRequest<UpdatePermissionCommand>(request.url, request);

    return await this._httpService.Put<UpdatePermissionCommand, ServiceResult<Permission>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
