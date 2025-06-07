import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Branch} from "../../../entities/branch";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreateBranchCommand extends IRequest<CreateBranchCommand, Branch> {
  public title:string | undefined = undefined;
  public parentId:number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Branch): CreateBranchCommand {
    throw new ApplicationError(CreateBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Branch {
    throw new ApplicationError(CreateBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/branch/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBranchCommandHandler.name)
export class CreateBranchCommandHandler implements IRequestHandler<CreateBranchCommand, Branch> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateBranchCommand): Promise<Branch> {
    let httpRequest: HttpRequest<CreateBranchCommand> = new HttpRequest<CreateBranchCommand>(request.url, request);

    return await this._httpService.Post<CreateBranchCommand, ServiceResult<Branch>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
