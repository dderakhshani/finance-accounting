import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Position} from "../../../entities/position";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Inject} from "@angular/core";

export class UpdatePositionCommand extends IRequest<UpdatePositionCommand, Position> {
  public id: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public levelCode: number | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public title: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Position): UpdatePositionCommand {
    this.mapBasics(entity, this)
    return this;
  }

  mapTo(): Position {
    throw new ApplicationError(UpdatePositionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/position/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePositionCommandHandler.name)
export class UpdatePositionCommandHandler implements IRequestHandler<UpdatePositionCommand, Position> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdatePositionCommand): Promise<Position> {
    let httpRequest: HttpRequest<UpdatePositionCommand> = new HttpRequest<UpdatePositionCommand>(request.url, request);


    return await this._httpService.Put<UpdatePositionCommand, ServiceResult<Position>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
