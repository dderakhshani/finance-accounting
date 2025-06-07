import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";

import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { PagesCommonService } from "../../../../shared/services/pages/pages-common.service";
import { spCommodityCost } from "../../entities/sp-commodity-cost";
import { PaginatedList } from "../../../../core/models/paginated-list";

export class GetCommodityCostQuery extends IRequest<GetCommodityCostQuery,  PaginatedList<spCommodityCost>> {
  constructor(
    
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public warhouseId: string | undefined = undefined,
   
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity:  PaginatedList<spCommodityCost>): GetCommodityCostQuery {
    throw new ApplicationError(GetCommodityCostQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo():  PaginatedList<spCommodityCost> {
    throw new ApplicationError(GetCommodityCostQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetCommodityCost";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCostQueryHandler.name)
export class GetCommodityCostQueryHandler implements IRequestHandler<GetCommodityCostQuery,  PaginatedList<spCommodityCost>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetCommodityCostQuery): Promise< PaginatedList<spCommodityCost>> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<GetCommodityCostQuery> = new HttpRequest<GetCommodityCostQuery>(request.url, request);

    httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;
    if (request.warhouseId != undefined) {
      httpRequest.Query += `&warehouseId=${request.warhouseId}`
    }
    
    return await this._httpService.Post<GetCommodityCostQuery, ServiceResult< PaginatedList<spCommodityCost>>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
