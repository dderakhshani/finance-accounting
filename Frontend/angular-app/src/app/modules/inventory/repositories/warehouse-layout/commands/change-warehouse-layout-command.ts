import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {WarehouseLayout} from "../../../entities/warehouse-layout";

export class ChangeWarehouseLayoutCommand extends IRequest<ChangeWarehouseLayoutCommand, WarehouseLayout> {

  public currentWarehouseLayoutId: number | undefined = undefined;
  public warehouseLayoutId: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  //تعداد قابل جابجایی
  public capacity: number | undefined = undefined;
  //تعداد جابجایی
  public quantity: number | undefined = undefined;
  public commodityId: number | undefined = undefined;

  public warehouseLayoutTitle: string | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public warehouseLayoutCapacity: number | undefined = undefined;
  public warehouseLayoutAvailableQuantity: number | undefined = undefined;

  constructor() {
    super();
  }


  mapFrom(entity: WarehouseLayout): ChangeWarehouseLayoutCommand {
    this.mapBasics(entity, this)



    return this;
  }
  mapTo(): WarehouseLayout {
    throw new ApplicationError(ChangeWarehouseLayoutCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/WarehouseOperations/ChangePlacementWarehouseDirectReceipt";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ChangeWarehouseLayoutCommandHandler.name)
export class ChangeWarehouseLayoutCommandHandler implements IRequestHandler<ChangeWarehouseLayoutCommand, WarehouseLayout> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: ChangeWarehouseLayoutCommand): Promise<WarehouseLayout> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<ChangeWarehouseLayoutCommand> = new HttpRequest<ChangeWarehouseLayoutCommand>(request.url, request);



    return await this._httpService.Post<ChangeWarehouseLayoutCommand, ServiceResult<WarehouseLayout>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
