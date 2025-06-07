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
import { spCommodityReports } from "../../entities/spCommodityReports";


export class GetCommodityReportsRialQuery extends IRequest<GetCommodityReportsRialQuery, spCommodityReports[]> {


  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public CommodityId: number | undefined = undefined,
    public AccountHeadId: string | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    
    public conditions?: SearchQuery[]
   
  ) {
    super();
  }

  mapFrom(entity: spCommodityReports[]): GetCommodityReportsRialQuery {
    throw new ApplicationError(GetCommodityReportsRialQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spCommodityReports[] {
    throw new ApplicationError(GetCommodityReportsRialQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetCommodityReportsRial";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityReportsRialQueryHandler.name)
export class GetCommodityReportsRialQueryHandler implements IRequestHandler<GetCommodityReportsRialQuery, spCommodityReports[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityReportsRialQuery): Promise<spCommodityReports[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCommodityReportsRialQuery> = new HttpRequest<GetCommodityReportsRialQuery>(request.url, request);

   
    if (request.CommodityId != undefined)
      httpRequest.Query += `CommodityId=${request.CommodityId}`
    if (request.AccountHeadId != undefined)
      httpRequest.Query += `&AccountHeadId=${request.AccountHeadId}`;
    if (request.fromDate != undefined) {
      httpRequest.Query += `&fromDate=${request.fromDate?.toUTCString()}`;
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `&toDate=${request.toDate?.toUTCString()}`;
    }
    
    
      return await this._httpService.Post<GetCommodityReportsRialQuery, ServiceResult<spCommodityReports[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
