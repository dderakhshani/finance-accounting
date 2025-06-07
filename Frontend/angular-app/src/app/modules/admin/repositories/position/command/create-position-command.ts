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

export class CreatePositionCommand extends IRequest<CreatePositionCommand, Position> {

  public parentId:number | undefined = undefined;
  public levelCode:number | undefined = undefined;
  public uniqueName:string | undefined = undefined;
  public title:string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Position): CreatePositionCommand {
    throw new ApplicationError(CreatePositionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Position {
    throw new ApplicationError(CreatePositionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/position/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePositionCommandHandler.name)
export class CreatePositionCommandHandler implements IRequestHandler<CreatePositionCommand, Position> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreatePositionCommand): Promise<Position> {
    let httpRequest: HttpRequest<CreatePositionCommand> = new HttpRequest<CreatePositionCommand>(request.url, request);


    return await this._httpService.Post<CreatePositionCommand, ServiceResult<Position>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
