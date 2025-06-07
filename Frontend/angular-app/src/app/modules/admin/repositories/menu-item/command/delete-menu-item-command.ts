import {Inject} from "@angular/core";
import {MenuItem} from "../../../entities/menu-item";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteMenuItemCommand extends IRequest<DeleteMenuItemCommand, MenuItem> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: MenuItem): DeleteMenuItemCommand {
    throw new ApplicationError(DeleteMenuItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MenuItem {
    throw new ApplicationError(DeleteMenuItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/menuItem/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteMenuItemCommandHandler.name)
export class DeleteMenuItemCommandHandler implements IRequestHandler<DeleteMenuItemCommand, MenuItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteMenuItemCommand): Promise<MenuItem> {
    let httpRequest: HttpRequest<DeleteMenuItemCommand> = new HttpRequest<DeleteMenuItemCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Delete<ServiceResult<MenuItem>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
