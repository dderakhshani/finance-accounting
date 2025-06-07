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


export class PlacementWarehouseDirectReceiptCommand extends IRequest<PlacementWarehouseDirectReceiptCommand, Receipt> {


  //public id: number | undefined = undefined;
  // public codeVoucherGroupId: number | undefined = undefined;

  public id: number | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: Receipt): PlacementWarehouseDirectReceiptCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(PlacementWarehouseDirectReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseOperations/PlacementWarehouseDirectReceipt";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(PlacementWarehouseDirectReceiptCommandHandler.name)
export class PlacementWarehouseDirectReceiptCommandHandler implements IRequestHandler<PlacementWarehouseDirectReceiptCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: PlacementWarehouseDirectReceiptCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<PlacementWarehouseDirectReceiptCommand> = new HttpRequest<PlacementWarehouseDirectReceiptCommand>(request.url, request);


    return await this._httpService.Post<PlacementWarehouseDirectReceiptCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
