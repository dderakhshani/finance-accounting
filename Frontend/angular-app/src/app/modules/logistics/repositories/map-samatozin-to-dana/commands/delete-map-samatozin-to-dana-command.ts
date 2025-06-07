import {Inject} from "@angular/core";

import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { MapSamatozinToDana } from "../../../entities/map-samatozin-to-dana";

export class DeleteMapSamatozinToDanaCommand extends IRequest<DeleteMapSamatozinToDanaCommand, MapSamatozinToDana> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: MapSamatozinToDana): DeleteMapSamatozinToDanaCommand {
    throw new ApplicationError(DeleteMapSamatozinToDanaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MapSamatozinToDana {
    throw new ApplicationError(DeleteMapSamatozinToDanaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/logistics/MapSamatozinToDana/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteMapSamatozinToDanaCommandHandler.name)
export class DeleteMapSamatozinToDanaCommandHandler implements IRequestHandler<DeleteMapSamatozinToDanaCommand, MapSamatozinToDana> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteMapSamatozinToDanaCommand): Promise<MapSamatozinToDana> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<DeleteMapSamatozinToDanaCommand> = new HttpRequest<DeleteMapSamatozinToDanaCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Delete<ServiceResult<MapSamatozinToDana>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
