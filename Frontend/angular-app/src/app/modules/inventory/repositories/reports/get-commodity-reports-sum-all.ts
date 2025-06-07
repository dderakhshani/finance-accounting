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
import { spCommodityReportsSumAll } from "../../entities/spCommodityReports";


export class GetCommodityReportsSumAllQuery extends IRequest<GetCommodityReportsSumAllQuery, spCommodityReportsSumAll[]> {


  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public CommodityId: number | undefined = undefined,
    public WarehouseId: string | undefined = undefined,
    public commodityTitle: string | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    
    public conditions?: SearchQuery[]
   
  ) {
    super();
  }

  mapFrom(entity: spCommodityReportsSumAll[]): GetCommodityReportsSumAllQuery {
    throw new ApplicationError(GetCommodityReportsSumAllQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spCommodityReportsSumAll[] {
    throw new ApplicationError(GetCommodityReportsSumAllQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetCommodityReportsSumAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityReportsSumAllQueryHandler.name)
export class GetCommodityReportsSumAllQueryHandler implements IRequestHandler<GetCommodityReportsSumAllQuery, spCommodityReportsSumAll[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityReportsSumAllQuery): Promise<spCommodityReportsSumAll[]> {
   
    let httpRequest: HttpRequest<GetCommodityReportsSumAllQuery> = new HttpRequest<GetCommodityReportsSumAllQuery>(request.url, request);

   
    if (request.CommodityId != undefined)
      httpRequest.Query += `CommodityId=${request.CommodityId}`
    if (request.WarehouseId != undefined)
      httpRequest.Query += `&WarehouseId=${request.WarehouseId}`;
    if (request.fromDate != undefined) {
      httpRequest.Query += `&fromDate=${request.fromDate?.toUTCString()}`;
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `&toDate=${request.toDate?.toUTCString()}`;
    }
    if (request.commodityTitle != undefined) {
      httpRequest.Query += `&commodityTitle=${request.commodityTitle}`;
    }
    
    
    return await this._httpService.Post<GetCommodityReportsSumAllQuery, ServiceResult<spCommodityReportsSumAll[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
