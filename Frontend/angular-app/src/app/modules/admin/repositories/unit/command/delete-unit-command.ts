import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Unit} from "../../../entities/unit";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";

export class DeleteUnitCommand extends IRequest<DeleteUnitCommand, Unit> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Unit): DeleteUnitCommand {
    throw new ApplicationError(DeleteUnitCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Unit {
    throw new ApplicationError(DeleteUnitCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/unit/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteUnitCommandHandler.name)
export class DeleteUnitCommandHandler implements IRequestHandler<DeleteUnitCommand, Unit> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteUnitCommand): Promise<Unit> {
    let httpRequest: HttpRequest<DeleteUnitCommand> = new HttpRequest<DeleteUnitCommand>(request.url, request);
    httpRequest.Query  +=  `id=${request.entityId}` ;

    return await this._httpService.Delete<ServiceResult<Unit>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
