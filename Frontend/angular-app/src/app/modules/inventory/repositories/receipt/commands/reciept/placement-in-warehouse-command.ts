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


export class PlacementWarehouseCommand extends IRequest<PlacementWarehouseCommand, Receipt> {


  //public id: number | undefined = undefined;
  // public codeVoucherGroupId: number | undefined = undefined;

  public id: number | undefined = undefined;
  public documentItemId: number | undefined = undefined;
  public CommodityId: number | undefined = undefined;
  public Quantity: number | undefined = undefined;
  public WarehouseLayoutId: number | undefined = undefined;
  public WarehouseLayoutQuantityId: number | undefined = undefined;

  public warehouseId: number | undefined = undefined;
  public warehouseId_Old: number | undefined = undefined;
  public warehouseLayoutQuantityId_Old: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Receipt): PlacementWarehouseCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(PlacementWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseOperations/PlacementWarehouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(PlacementWarehouseCommandHandler.name)
export class PlacementWarehouseCommandHandler implements IRequestHandler<PlacementWarehouseCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: PlacementWarehouseCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<PlacementWarehouseCommand> = new HttpRequest<PlacementWarehouseCommand>(request.url, request);


    return await this._httpService.Post<PlacementWarehouseCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
