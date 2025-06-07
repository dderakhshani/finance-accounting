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

export class CreateMapSamatozinToDanaCommand extends IRequest<CreateMapSamatozinToDanaCommand, MapSamatozinToDana> {

  public accountReferenceId: number | undefined = undefined;
  public accountReferenceCode: string | undefined = undefined;
  public samaTozinCode: number | undefined = undefined;
  public samaTozinTitle: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: MapSamatozinToDana): CreateMapSamatozinToDanaCommand {
    throw new ApplicationError(CreateMapSamatozinToDanaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MapSamatozinToDana {
    throw new ApplicationError(CreateMapSamatozinToDanaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/logistics/MapSamatozinToDana/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateMapSamatozinToDanaCommandHandler.name)
export class CreateMapSamatozinToDanaCommandHandler implements IRequestHandler<CreateMapSamatozinToDanaCommand, MapSamatozinToDana> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateMapSamatozinToDanaCommand): Promise<MapSamatozinToDana> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateMapSamatozinToDanaCommand> = new HttpRequest<CreateMapSamatozinToDanaCommand>(request.url, request);


    return await this._httpService.Post<CreateMapSamatozinToDanaCommand, ServiceResult<MapSamatozinToDana>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
