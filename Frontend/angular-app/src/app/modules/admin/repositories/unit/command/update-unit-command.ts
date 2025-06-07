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

export class UpdateUnitCommand extends IRequest<UpdateUnitCommand, Unit> {
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

  mapFrom(entity: Unit): UpdateUnitCommand {
    this.mapBasics(entity, this);
    this.positionIds = entity.positionIds ?? []
    return this;
  }

  mapTo(): Unit {
    throw new ApplicationError(UpdateUnitCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/unit/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateUnitCommandHandler.name)
export class UpdateUnitCommandHandler implements IRequestHandler<UpdateUnitCommand, Unit> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateUnitCommand): Promise<Unit> {
    let httpRequest: HttpRequest<UpdateUnitCommand> = new HttpRequest<UpdateUnitCommand>(request.url, request);




    return await this._httpService.Put<UpdateUnitCommand, ServiceResult<Unit>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
