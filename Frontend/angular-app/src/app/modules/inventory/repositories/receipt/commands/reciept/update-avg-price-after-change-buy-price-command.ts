import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";


export class UpdateAvgPriceAfterChangeBuyPriceCommand extends IRequest<UpdateAvgPriceAfterChangeBuyPriceCommand, UpdateAvgPriceAfterChangeBuyPriceCommand> {



  public documentId: number | undefined = undefined;
  public documentItemId: number | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: UpdateAvgPriceAfterChangeBuyPriceCommand): UpdateAvgPriceAfterChangeBuyPriceCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): UpdateAvgPriceAfterChangeBuyPriceCommand {

    throw new ApplicationError(UpdateAvgPriceAfterChangeBuyPriceCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateAvgPriceAfterChangeBuyPrice";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAvgPriceAfterChangeBuyPriceCommandHandler.name)
export class UpdateAvgPriceAfterChangeBuyPriceCommandHandler implements IRequestHandler<UpdateAvgPriceAfterChangeBuyPriceCommand, UpdateAvgPriceAfterChangeBuyPriceCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateAvgPriceAfterChangeBuyPriceCommand): Promise<UpdateAvgPriceAfterChangeBuyPriceCommand> {
    
    let httpRequest: HttpRequest<UpdateAvgPriceAfterChangeBuyPriceCommand> = new HttpRequest<UpdateAvgPriceAfterChangeBuyPriceCommand>(request.url, request);


    return await this._httpService.Post<UpdateAvgPriceAfterChangeBuyPriceCommand, ServiceResult<UpdateAvgPriceAfterChangeBuyPriceCommand>>(httpRequest).toPromise().then(response => {

      return response.objResult
    })
  }
}
