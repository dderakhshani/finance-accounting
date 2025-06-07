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
import { spCommodityReceiptReports } from "../../entities/spCommodityReceiptReports";


export class GetCommodityReciptReportsQuery extends IRequest<GetCommodityReciptReportsQuery, spCommodityReceiptReports[]> {


  constructor(

    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public CommodityId: number | undefined = undefined,
    public WarehouseId: number | undefined = undefined,
    public documentNo: number | undefined = undefined,
    public requestNo: number | undefined = undefined,
    public commodityTitle: number | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[]

  ) {
    super();
  }

  mapFrom(entity: spCommodityReceiptReports[]): GetCommodityReciptReportsQuery {
    throw new ApplicationError(GetCommodityReciptReportsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spCommodityReceiptReports[] {
    throw new ApplicationError(GetCommodityReciptReportsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetCommodityReceiptReports";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityReciptReportsQueryHandler.name)
export class GetCommodityReciptReportsQueryHandler implements IRequestHandler<GetCommodityReciptReportsQuery, spCommodityReceiptReports[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityReciptReportsQuery): Promise<spCommodityReceiptReports[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCommodityReciptReportsQuery> = new HttpRequest<GetCommodityReciptReportsQuery>(request.url, request);

   
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
    if (request.documentNo != undefined) {
      httpRequest.Query += `&DocumentNo=${request.documentNo}`;
    }
    if (request.requestNo != undefined) {
      httpRequest.Query += `&requestNo=${request.requestNo}`;
    }
    if (request.commodityTitle != undefined) {
      httpRequest.Query += `&commodityTitle=${request.commodityTitle}`;
    }
   
   
    return await this._httpService.Post<GetCommodityReciptReportsQuery, ServiceResult<spCommodityReceiptReports[]>>(httpRequest).toPromise().then(response => {
    
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
