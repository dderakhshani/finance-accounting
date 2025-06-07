import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { Receipt } from "../../../../entities/receipt";

export class GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery extends IRequest<GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery, Receipt> {
  constructor(
    public documentNo: number,
    public warehouseId: any,
    
  ) {
    super();
  }


  mapFrom(entity: Receipt): GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery {
    throw new ApplicationError(GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/TemporaryReceipt/GetByDocumentNoWithWarehouseId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetTemporaryRecepitByDocumentWithWarehouseIdNoQueryHandler.name)
export class GetTemporaryRecepitByDocumentWithWarehouseIdNoQueryHandler implements IRequestHandler<GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery> = new HttpRequest<GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery>(request.url, request);

    httpRequest.Query += `DocumentNo=${request.documentNo}`;
    if (request.warehouseId != undefined) {
      httpRequest.Query += `&warehouseId=${request.warehouseId}`;
    }

    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
     

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
