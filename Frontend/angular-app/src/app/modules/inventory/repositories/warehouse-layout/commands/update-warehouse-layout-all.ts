import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";



export class UpdateWarehouseLayoutALLCommand extends IRequest<UpdateWarehouseLayoutALLCommand, any> {
  
  public warehouseId: number | undefined = undefined;
  
  constructor() {
    super();
  }

  mapFrom(entity: any): UpdateWarehouseLayoutALLCommand {

    return this;
  }

  mapTo(): any {

    throw new ApplicationError(UpdateWarehouseLayoutALLCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseOperations/UpdateWarehouseLayoutALL";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateWarehouseLayoutALLCommandHandler.name)
export class UpdateWarehouseLayoutALLCommandHandler implements IRequestHandler<UpdateWarehouseLayoutALLCommand, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateWarehouseLayoutALLCommand): Promise<any> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateWarehouseLayoutALLCommand> = new HttpRequest<UpdateWarehouseLayoutALLCommand>(request.url, request);


    return await this._httpService.Post<UpdateWarehouseLayoutALLCommand, ServiceResult<any>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
