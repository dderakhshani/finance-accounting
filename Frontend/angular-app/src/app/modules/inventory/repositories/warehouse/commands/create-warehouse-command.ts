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

export class CreateWarehouseCommand extends IRequest<CreateWarehouseCommand, Warehouse> {



  public parentId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public isActive: boolean | undefined = true;
  public accountHeadId: number | undefined = undefined;
  public commodityCategoryId: number | undefined = undefined;
  public accessPermission: string | undefined = undefined;
  public CommodityCategories: CommodityCategory[] = [];
  public ReceiptAllStatus: ReceiptAllStatusModel[] = [];
  public sort : number | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: Warehouse): CreateWarehouseCommand {
    throw new ApplicationError(CreateWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Warehouse {
    throw new ApplicationError(CreateWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/Warehouse/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateWarehouseCommandHandler.name)
export class CreateWarehouseCommandHandler implements IRequestHandler<CreateWarehouseCommand, Warehouse> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateWarehouseCommand): Promise<Warehouse> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateWarehouseCommand> = new HttpRequest<CreateWarehouseCommand>(request.url, request);


    return await this._httpService.Post<CreateWarehouseCommand, ServiceResult<Warehouse>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
