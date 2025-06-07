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



export class UpdateWarehouseCardexCommand extends IRequest<UpdateWarehouseCardexCommand, Receipt> {
  
  
 
  public warehouseId: number | undefined = undefined;
  public yearId: number | undefined = undefined;

  
  constructor() {
    super();
  }

  mapFrom(entity: Receipt): UpdateWarehouseCardexCommand {

    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(UpdateWarehouseCardexCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateWarehouseCardex";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateWarehouseCardexCommandHandler.name)
export class UpdateWarehouseCardexCommandHandler implements IRequestHandler<UpdateWarehouseCardexCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateWarehouseCardexCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateWarehouseCardexCommand> = new HttpRequest<UpdateWarehouseCardexCommand>(request.url, request);


    return await this._httpService.Post<UpdateWarehouseCardexCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
