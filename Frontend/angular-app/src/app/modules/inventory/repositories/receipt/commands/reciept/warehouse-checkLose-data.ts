import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";



export class WarehouseCheckLoseDataCommand extends IRequest<WarehouseCheckLoseDataCommand, any> {
  
  
  constructor() {
    super();
  }

  mapFrom(entity: any): WarehouseCheckLoseDataCommand {

    return this;
  }

  mapTo(): any {

    throw new ApplicationError(WarehouseCheckLoseDataCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/WarehouseCheckLoseData";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(WarehouseCheckLoseDataCommandHandler.name)
export class WarehouseCheckLoseDataCommandHandler implements IRequestHandler<WarehouseCheckLoseDataCommand, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: WarehouseCheckLoseDataCommand): Promise<any> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<WarehouseCheckLoseDataCommand> = new HttpRequest<WarehouseCheckLoseDataCommand>(request.url, request);


    return await this._httpService.Post<WarehouseCheckLoseDataCommand, ServiceResult<any>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
