import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";

import { spGetMonthlyEntryToWarehouse } from "../../entities/spGetMonthlyEntryToWarehouse";


export class GetMonthlyEntryToWarehouseQuery extends IRequest<GetMonthlyEntryToWarehouseQuery, spGetMonthlyEntryToWarehouse[]> {


  constructor(
    public CommodityId: number | undefined = undefined,
    public WarehouseId: number | undefined = undefined,
    public YearId: number | undefined = undefined,
    public enterMode: number | undefined = undefined,


  ) {
    super();
  }

  mapFrom(entity: spGetMonthlyEntryToWarehouse[]): GetMonthlyEntryToWarehouseQuery {
    throw new ApplicationError(GetMonthlyEntryToWarehouseQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spGetMonthlyEntryToWarehouse[] {
    throw new ApplicationError(GetMonthlyEntryToWarehouseQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetMonthlyEntryToWarehouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMonthlyEntryToWarehouseQueryHandler.name)
export class GetMonthlyEntryToWarehouseQueryHandler implements IRequestHandler<GetMonthlyEntryToWarehouseQuery, spGetMonthlyEntryToWarehouse[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetMonthlyEntryToWarehouseQuery): Promise<spGetMonthlyEntryToWarehouse[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetMonthlyEntryToWarehouseQuery> = new HttpRequest<GetMonthlyEntryToWarehouseQuery>(request.url, request);

   
    if (request.CommodityId != undefined)
      httpRequest.Query += `CommodityId=${request.CommodityId}`
    if (request.WarehouseId != undefined)
      httpRequest.Query += `&WarehouseId=${request.WarehouseId}`;
    if (request.YearId != undefined)
      httpRequest.Query += `&YearId=${request.YearId}`;
    if (request.enterMode != undefined)
      httpRequest.Query += `&enterMode=${request.enterMode}`;

    return await this._httpService.Get<ServiceResult<spGetMonthlyEntryToWarehouse[]>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
