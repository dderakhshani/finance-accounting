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
import { ReceiptItem } from "../../../../entities/receipt-item";

export class GetStartReceiptsItemQuery extends IRequest<GetStartReceiptsItemQuery, ReceiptItem> {
  constructor(public commodityId: number,public warehouseId: number) {
    super();
  }


  mapFrom(entity: ReceiptItem): GetStartReceiptsItemQuery {
    throw new ApplicationError(GetStartReceiptsItemQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ReceiptItem {
    throw new ApplicationError(GetStartReceiptsItemQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetStartReceiptsItem";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetStartReceiptsItemQueryHandler.name)
export class GetStartReceiptsItemQueryHandler implements IRequestHandler<GetStartReceiptsItemQuery, ReceiptItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetStartReceiptsItemQuery): Promise<ReceiptItem> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetStartReceiptsItemQuery> = new HttpRequest<GetStartReceiptsItemQuery>(request.url, request);
    httpRequest.Query += `commodityId=${request.commodityId}&warehouseId=${request.warehouseId}`;


    return await this._httpService.Get<ServiceResult<ReceiptItem>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
