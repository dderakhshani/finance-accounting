import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { Help } from "../../../entities/help";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { Inject } from "@angular/core";
import { HttpService } from "../../../../../core/services/http/http.service";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "../../../../../core/models/service-result";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class DeleteHelpCommand extends IRequest<DeleteHelpCommand, Help>{

  constructor(
    public entityId: number
  ) {
    super();
  }

  mapFrom(entity: Help): DeleteHelpCommand {
    throw new ApplicationError(DeleteHelpCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Help {
    throw new ApplicationError(DeleteHelpCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/help/delete"
  }

  get validationRules() : ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteHelpCommandHandler.name)
export class DeleteHelpCommandHandler implements IRequestHandler<DeleteHelpCommand, Help> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

) {
  }
  async Handle(request: DeleteHelpCommand): Promise<Help>{
    let httpRequest: HttpRequest<DeleteHelpCommand> = new HttpRequest<DeleteHelpCommand>(request.url, request);
    httpRequest.Query += `id=${request.entityId}`

    return await this._httpService.Delete<ServiceResult<Help>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
