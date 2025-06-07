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
export class TermsOfAccess {
  constructor(field: string, oparation: string, value: string, composition: string) {
    this.field = field;
    this.oparation = oparation;
    this.value = value;
    this.composition = composition;
  }
  public field: string | undefined = undefined;
  public oparation: string | undefined = undefined;
  public value: string | undefined = undefined;
  public composition: string | undefined = undefined;
}
export class CreatePermissionCommand extends IRequest<CreatePermissionCommand, Permission> {

  public parentId: number | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public title: string | undefined = undefined;
  public isDataRowLimiter: boolean | undefined = undefined;
  public subSystem: string | undefined = undefined;

  public tableName: string | undefined = undefined;
  public termsOfAccesses: TermsOfAccess[] | undefined = undefined;
  public accessToAll: boolean | undefined = false;

  constructor() {
    super();
  }

  mapFrom(entity: Permission): CreatePermissionCommand {
    throw new ApplicationError(CreatePermissionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Permission {
    throw new ApplicationError(CreatePermissionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/permission/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePermissionCommandHandler.name)
export class CreatePermissionCommandHandler implements IRequestHandler<CreatePermissionCommand, Permission> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreatePermissionCommand): Promise<Permission> {
    let httpRequest: HttpRequest<CreatePermissionCommand> = new HttpRequest<CreatePermissionCommand>(request.url, request);

    return await this._httpService.Post<CreatePermissionCommand, ServiceResult<Permission>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
