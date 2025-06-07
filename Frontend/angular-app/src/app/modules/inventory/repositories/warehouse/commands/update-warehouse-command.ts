import { Inject } from "@angular/core";
import { Warehouse } from "../../../entities/warehouse";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { CommodityCategory } from "../../../../commodity/entities/commodity-category";
import { ReceiptAllStatusModel } from "../../../entities/receipt-all-status";

export class UpdateWarehouseCommand extends IRequest<UpdateWarehouseCommand, Warehouse> {


  public id: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public isActive: boolean | undefined = undefined;
  public accountHeadId: number | undefined = undefined;
  public commodityCategoryId: number | undefined = undefined;
  public accessPermission: string | undefined = undefined;
  public sort: number | undefined = undefined;
  public CommodityCategories: CommodityCategory[] = [];
  public ReceiptAllStatus: ReceiptAllStatusModel[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: Warehouse): UpdateWarehouseCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): Warehouse {

    throw new ApplicationError(UpdateWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/Warehouse/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateWarehouseCommandHandler.name)
export class UpdateWarehouseCommandHandler implements IRequestHandler<UpdateWarehouseCommand, Warehouse> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateWarehouseCommand): Promise<Warehouse> {
    this._notificationService.isLoader = true;

    let httpRequest: HttpRequest<UpdateWarehouseCommand> = new HttpRequest<UpdateWarehouseCommand>(request.url, request);




    return await this._httpService.Put<UpdateWarehouseCommand, ServiceResult<Warehouse>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
