import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";



export class RemoveCommodityFromWarehouseCommand extends IRequest<RemoveCommodityFromWarehouseCommand, any> {
  
  public warehouseId: number | undefined = undefined;
  
  constructor() {
    super();
  }

  mapFrom(entity: any): RemoveCommodityFromWarehouseCommand {

    return this;
  }

  mapTo(): any {

    throw new ApplicationError(RemoveCommodityFromWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseOperations/RemoveCommodityFromWarehouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(RemoveCommodityFromWarehouseCommandHandler.name)
export class RemoveCommodityFromWarehouseCommandHandler implements IRequestHandler<RemoveCommodityFromWarehouseCommand, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: RemoveCommodityFromWarehouseCommand): Promise<any> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<RemoveCommodityFromWarehouseCommand> = new HttpRequest<RemoveCommodityFromWarehouseCommand>(request.url, request);


    return await this._httpService.Post<RemoveCommodityFromWarehouseCommand, ServiceResult<any>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
