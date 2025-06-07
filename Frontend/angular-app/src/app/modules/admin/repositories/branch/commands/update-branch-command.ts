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

export class UpdateBranchCommand extends IRequest<UpdateBranchCommand, Branch> {
  public id:number | undefined = undefined;
  public title:string | undefined = undefined;
  public parentId:number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Branch): UpdateBranchCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): Branch {
    throw new ApplicationError(UpdateBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/branch/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateBranchCommandHandler.name)
export class UpdateBranchCommandHandler implements IRequestHandler<UpdateBranchCommand, Branch> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateBranchCommand): Promise<Branch> {
    let httpRequest: HttpRequest<UpdateBranchCommand> = new HttpRequest<UpdateBranchCommand>(request.url, request);

    return await this._httpService.Put<UpdateBranchCommand, ServiceResult<Branch>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
