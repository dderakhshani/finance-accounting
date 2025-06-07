import {Inject} from "@angular/core";
import {Receipt} from "../../../../entities/receipt";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";


export class UpdateIsPlacementCompleteCommand extends IRequest<UpdateIsPlacementCompleteCommand, UpdateIsPlacementCompleteCommand> {



  public id: number | undefined = undefined;
  public isPlacementComplete: boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: UpdateIsPlacementCompleteCommand): UpdateIsPlacementCompleteCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): UpdateIsPlacementCompleteCommand {

    throw new ApplicationError(UpdateIsPlacementCompleteCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateIsPlacementCompleteReceipt";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateIsPlacementCompleteCommandHandler.name)
export class UpdateIsPlacementCompleteCommandHandler implements IRequestHandler<UpdateIsPlacementCompleteCommand, UpdateIsPlacementCompleteCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateIsPlacementCompleteCommand): Promise<UpdateIsPlacementCompleteCommand> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateIsPlacementCompleteCommand> = new HttpRequest<UpdateIsPlacementCompleteCommand>(request.url, request);


    return await this._httpService.Post<UpdateIsPlacementCompleteCommand, ServiceResult<UpdateIsPlacementCompleteCommand>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
