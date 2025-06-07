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

export class CreateUnitCommand extends IRequest<CreateUnitCommand, Unit> {
  public id:number| undefined = undefined;
  public title:string| undefined = undefined;
  public levelCode:string| undefined = undefined;
  public parentId:number| undefined = undefined;
  public branchId:number| undefined = undefined;
  public positionIds:number[]=[];
  public cloneFromUnit: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Unit): CreateUnitCommand {
    throw new ApplicationError(CreateUnitCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Unit {
    throw new ApplicationError(CreateUnitCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/unit/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateUnitCommandHandler.name)
export class CreateUnitCommandHandler implements IRequestHandler<CreateUnitCommand, Unit> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateUnitCommand): Promise<Unit> {
    let httpRequest: HttpRequest<CreateUnitCommand> = new HttpRequest<CreateUnitCommand>(request.url, request);

    return await this._httpService.Post<CreateUnitCommand, ServiceResult<Unit>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })


  }
}
