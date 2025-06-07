import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Position} from "../../../entities/position";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";

export class DeletePositionCommand extends IRequest<DeletePositionCommand, Position> {

  constructor( public entityId:number ) {
    super();
  }

  mapFrom(entity: Position): DeletePositionCommand {
    throw new ApplicationError(DeletePositionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Position {
    throw new ApplicationError(DeletePositionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/position/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePositionCommandHandler.name)
export class DeletePositionCommandHandler implements IRequestHandler<DeletePositionCommand, Position> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeletePositionCommand): Promise<Position> {
    let httpRequest: HttpRequest<DeletePositionCommand> = new HttpRequest<DeletePositionCommand>(request.url, request);
    httpRequest.Query  +=  `id=${request.entityId}`  ;



    return await this._httpService.Delete<ServiceResult<Position>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
