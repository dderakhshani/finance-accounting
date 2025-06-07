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

export class DeleteBranchCommand extends IRequest<DeleteBranchCommand, Branch> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Branch): DeleteBranchCommand {
    throw new ApplicationError(DeleteBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Branch {
    throw new ApplicationError(DeleteBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/branch/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteBranchCommandHandler.name)
export class DeleteBranchCommandHandler implements IRequestHandler<DeleteBranchCommand, Branch> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteBranchCommand): Promise<Branch> {
    let httpRequest: HttpRequest<DeleteBranchCommand> = new HttpRequest<DeleteBranchCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<Branch>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
