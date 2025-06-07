import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { WarehouseLayout } from "../../../../entities/warehouse-layout";
import { Receipt } from "../../../../entities/receipt";
export class LeavingPartWarehouseDocumentItem {
  commodityId: number | undefined|null = undefined;
  quantity: number | undefined | null = undefined;
  warehouseLayoutQuantitiyId: number | undefined | null = undefined;
  quantityTotal: number | undefined | null = undefined;
  requestItemId: number | undefined | null = undefined;
  description: string | undefined | null = undefined;
  quantityLayout: number | undefined | null = undefined;
}

export class LeavingPartWarehouseCommand extends IRequest<LeavingPartWarehouseCommand, Receipt> {

  public warehouseId: number | undefined = undefined;
  public request_No: number | undefined = undefined;
  public documentNo: number | undefined = undefined;
  
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public debitAccountHeadId: number | undefined = undefined;
  public isDocumentIssuance: boolean | undefined = undefined;
  public warehouseDocumentItem: LeavingPartWarehouseDocumentItem[] =[];
  constructor() {
    super();
  }


  mapFrom(entity: Receipt): LeavingPartWarehouseCommand {
    this.mapBasics(entity, this)



    return this;
  }
  mapTo(): Receipt {
    throw new ApplicationError(Receipt.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/LeavingWarehouse/LeavingPartWarehouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(LeavingPartWarehouseCommandHandler.name)
export class LeavingPartWarehouseCommandHandler implements IRequestHandler<LeavingPartWarehouseCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: LeavingPartWarehouseCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<LeavingPartWarehouseCommand> = new HttpRequest<LeavingPartWarehouseCommand>(request.url, request);



    return await this._httpService.Post<LeavingPartWarehouseCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
