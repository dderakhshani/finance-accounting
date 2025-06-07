import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../core/models/paginated-list";

import { PagesCommonService } from "../../../../shared/services/pages/pages-common.service";
import { spCommodityReportsWithWarehouse } from "../../entities/spCommodityReports";



export class GetCommodityReportsWithWarehouseQuery extends IRequest<GetCommodityReportsWithWarehouseQuery, PaginatedList<spCommodityReportsWithWarehouse>> {

  constructor(
   
   
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public warehouseId: string | undefined = undefined,
    public codeVoucherGroupId : number | undefined = undefined,
    public documentStauseBaseValue: number | undefined = undefined,
   
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<spCommodityReportsWithWarehouse>): GetCommodityReportsWithWarehouseQuery {
    throw new ApplicationError(GetCommodityReportsWithWarehouseQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<spCommodityReportsWithWarehouse> {
    throw new ApplicationError(GetCommodityReportsWithWarehouseQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetCommodityReportsWithWarehouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetCommodityReportsWithWarehouseQueryHandler.name)
export class GetCommodityReportsWithWarehouseQueryHandler implements IRequestHandler<GetCommodityReportsWithWarehouseQuery, PaginatedList<spCommodityReportsWithWarehouse>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetCommodityReportsWithWarehouseQuery): Promise<PaginatedList<spCommodityReportsWithWarehouse>> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<GetCommodityReportsWithWarehouseQuery> = new HttpRequest<GetCommodityReportsWithWarehouseQuery>(request.url, request);

    
    if (request.fromDate != undefined) {
      httpRequest.Query += `fromDate=${request.fromDate?.toUTCString() }&`
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `toDate=${request.toDate?.toUTCString() }&`
    }
    if (request.codeVoucherGroupId != undefined) {
      httpRequest.Query += `codeVoucherGroupId=${request.codeVoucherGroupId}&`
    }
    if (request.documentStauseBaseValue != undefined) {
      httpRequest.Query += `documentStauseBaseValue=${request.documentStauseBaseValue}&`
    }
    if (request.warehouseId != undefined) {
      httpRequest.Query += `warehouseId=${request.warehouseId}&`
    }
   
    return await this._httpService.Post<GetCommodityReportsWithWarehouseQuery, ServiceResult<PaginatedList<spCommodityReportsWithWarehouse>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
