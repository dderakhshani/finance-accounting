import { Inject } from "@angular/core";

import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { MapSamatozinToDana } from "../../../entities/map-samatozin-to-dana";


export class UpdateMapSamatozinToDanaCommand extends IRequest<UpdateMapSamatozinToDanaCommand, MapSamatozinToDana> {

  public id: number | undefined = undefined;
  public accountReferenceId: number | undefined = undefined;
  public accountReferenceCode: string | undefined = undefined;
  public samaTozinCode: number | undefined = undefined;
  public samaTozinTitle: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: MapSamatozinToDana): UpdateMapSamatozinToDanaCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): MapSamatozinToDana {

    throw new ApplicationError(UpdateMapSamatozinToDanaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/logistics/MapSamatozinToDana/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateMapSamatozinToDanaCommandHandler.name)
export class UpdateMapSamatozinToDanaCommandHandler implements IRequestHandler<UpdateMapSamatozinToDanaCommand, MapSamatozinToDana> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateMapSamatozinToDanaCommand): Promise<MapSamatozinToDana> {
    this._notificationService.isLoader = true;

    let httpRequest: HttpRequest<UpdateMapSamatozinToDanaCommand> = new HttpRequest<UpdateMapSamatozinToDanaCommand>(request.url, request);




    return await this._httpService.Put<UpdateMapSamatozinToDanaCommand, ServiceResult<MapSamatozinToDana>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
