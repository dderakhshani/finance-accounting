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


export class UpdateQuantityCommand extends IRequest<UpdateQuantityCommand, UpdateQuantityCommand> {



  public id: number | undefined = undefined;
  public quantity: number | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  

  constructor() {
    super();
  }

  mapFrom(entity: UpdateQuantityCommand): UpdateQuantityCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): UpdateQuantityCommand {

    throw new ApplicationError(UpdateQuantityCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateQuantity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateQuantityCommandHandler.name)
export class UpdateQuantityCommandHandler implements IRequestHandler<UpdateQuantityCommand, UpdateQuantityCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateQuantityCommand): Promise<UpdateQuantityCommand> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateQuantityCommand> = new HttpRequest<UpdateQuantityCommand>(request.url, request);


    return await this._httpService.Post<UpdateQuantityCommand, ServiceResult<UpdateQuantityCommand>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
