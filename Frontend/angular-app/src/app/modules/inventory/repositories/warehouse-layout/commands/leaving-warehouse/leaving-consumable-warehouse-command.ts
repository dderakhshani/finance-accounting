import { Inject } from "@angular/core";
import { Receipt } from "../../../../entities/receipt";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { LeavingDocumentItem } from "../../../../entities/leaving-cosumable-documentItem";



export class LeavingCosumableWarehouseCommand extends IRequest<LeavingCosumableWarehouseCommand, Receipt> {


  public id: number | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public documentNo: string | undefined = undefined;
  public isDocumentIssuance: boolean | undefined = true;
  public documentItems: LeavingDocumentItem[] = []
 
  constructor() {
    super();
  }

  mapFrom(entity: Receipt): LeavingCosumableWarehouseCommand {
    throw new Error("Method not implemented.");
  }

  mapTo(): Receipt {
    throw new ApplicationError(LeavingCosumableWarehouseCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    /*return "/inventory/LeavingWarehouse/LeavingCosumableWarehouse";*/
    return "/inventory/LeavingWarehouse/LeaveCommodity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(LeavingCosumableWarehouseCommandHandler.name)
export class LeavingCosumableWarehouseCommandHandler implements IRequestHandler<LeavingCosumableWarehouseCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: LeavingCosumableWarehouseCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<LeavingCosumableWarehouseCommand> = new HttpRequest<LeavingCosumableWarehouseCommand>(request.url, request);


    return await this._httpService.Post<LeavingCosumableWarehouseCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
