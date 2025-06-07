import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { PagesCommonService } from "../../../../shared/services/pages/pages-common.service";
import { Commodity } from "../../../accounting/entities/commodity";



export class GetQuantityCommodityQuery extends IRequest<Commodity, number> {
  

  constructor(public warehouseId: number,
    public commodityId: number = 0
    
    ) {
    super();
  }

  mapFrom(entity: number): Commodity {
      throw new Error("Method not implemented.");
  }

  mapTo(): number {
    throw new ApplicationError(GetQuantityCommodityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Commodity/GetQuantityCommodity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetQuantityCommodityQueryHandler.name)
export class GetQuantityCommodityQueryHandler implements IRequestHandler<GetQuantityCommodityQuery, number> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService
    
  ) {
  }

  async Handle(request: GetQuantityCommodityQuery): Promise<number> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetQuantityCommodityQuery> = new HttpRequest<GetQuantityCommodityQuery>(request.url, request);
    
    
    httpRequest.Query += `warehouseId=${request.warehouseId}&commodityId=${request.commodityId}`;

    return await this._httpService.Get<ServiceResult<number>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
 
}
