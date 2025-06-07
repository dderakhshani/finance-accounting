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


export class GetCommodityReceiptReportsRialQuery extends IRequest<GetCommodityReceiptReportsRialQuery, spCommodityReceiptReports[]> {


  constructor(

    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public CommodityId: number | undefined = undefined,
    public accountHeadId: number | undefined = undefined,
    public warehouseId: number | undefined = undefined,
    public documentNo: number | undefined = undefined,
   
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[]

  ) {
    super();
  }

  mapFrom(entity: spCommodityReceiptReports[]): GetCommodityReceiptReportsRialQuery {
    throw new ApplicationError(GetCommodityReceiptReportsRialQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spCommodityReceiptReports[] {
    throw new ApplicationError(GetCommodityReceiptReportsRialQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetCommodityReceiptReportsRial";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityReceiptReportsRialQueryHandler.name)
export class GetCommodityReceiptReportsRialQueryHandler implements IRequestHandler<GetCommodityReceiptReportsRialQuery, spCommodityReceiptReports[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityReceiptReportsRialQuery): Promise<spCommodityReceiptReports[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCommodityReceiptReportsRialQuery> = new HttpRequest<GetCommodityReceiptReportsRialQuery>(request.url, request);

   
    if (request.CommodityId != undefined)
      httpRequest.Query += `CommodityId=${request.CommodityId}`
    if (request.accountHeadId != undefined)
      httpRequest.Query += `&AccountHeadId=${request.accountHeadId}`;
    if (request.fromDate != undefined) {
      httpRequest.Query += `&fromDate=${request.fromDate?.toUTCString()}`;
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `&toDate=${request.toDate?.toUTCString()}`;
    }
    if (request.documentNo != undefined) {
      httpRequest.Query += `&DocumentNo=${request.documentNo}`;
    }
    if (request.warehouseId != undefined) {
      httpRequest.Query += `&warehouseId=${request.warehouseId}`;
    }
   
    
    return await this._httpService.Post<GetCommodityReceiptReportsRialQuery, ServiceResult<spCommodityReceiptReports[]>>(httpRequest).toPromise().then(response => {
    
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
