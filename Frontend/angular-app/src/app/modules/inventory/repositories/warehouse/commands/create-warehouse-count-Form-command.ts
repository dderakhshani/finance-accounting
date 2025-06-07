import {Inject} from "@angular/core";
import { Warehouse } from "../../../entities/warehouse";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { CommodityCategory } from "../../../../commodity/entities/commodity-category";
import { ReceiptAllStatusModel } from "../../../entities/receipt-all-status";
import {CountFormSelectedCommodity, WarehouseCountFormHead} from "../../../entities/warehouse-count-form-head";

export class CreateWarehouseCountFormCommand extends IRequest<CreateWarehouseCountFormCommand, WarehouseCountFormHead> {
  public parentId: number | undefined = undefined;
  public formDate: string  | undefined = undefined;
  public counterUserId: number| undefined = undefined;
  public confirmerUserId: number| undefined = undefined;
  public description: string| undefined = undefined;
  public warehouseLayoutId:number | undefined = undefined;
  public basedOnLocation:boolean | undefined = undefined;
  public warehouseId:number | undefined = undefined;
  public countFormSelectedCommodity:CountFormSelectedCommodity[]=[];

  constructor() {
    super();
  }

  mapFrom(entity: WarehouseCountFormHead): CreateWarehouseCountFormCommand {
    throw new ApplicationError(CreateWarehouseCountFormCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): WarehouseCountFormHead {
    throw new ApplicationError(CreateWarehouseCountFormCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/WarehouseCountForm/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateWarehouseCountFormCommandHandler.name)
export class CreateWarehouseCountFormCommandHandler implements IRequestHandler<CreateWarehouseCountFormCommand, WarehouseCountFormHead> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateWarehouseCountFormCommand): Promise<WarehouseCountFormHead> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateWarehouseCountFormCommand> = new HttpRequest<CreateWarehouseCountFormCommand>(request.url, request);


    return await this._httpService.Post<CreateWarehouseCountFormCommand, ServiceResult<WarehouseCountFormHead>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
