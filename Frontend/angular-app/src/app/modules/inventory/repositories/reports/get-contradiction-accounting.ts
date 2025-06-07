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
import { spContradictionDebit } from "../../entities/spContradictionDebit";


export class GetContradictionAccountingQuery extends IRequest<GetContradictionAccountingQuery, spContradictionDebit[]> {


  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public CommodityId: number | undefined = undefined,
    public accountHeadId: string | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    
    public conditions?: SearchQuery[]
   
  ) {
    super();
  }

  mapFrom(entity: spContradictionDebit[]): GetContradictionAccountingQuery {
    throw new ApplicationError(GetContradictionAccountingQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spContradictionDebit[] {
    throw new ApplicationError(GetContradictionAccountingQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetContradictionAccounting";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetContradictionAccountingQueryHandler.name)
export class GetContradictionAccountingQueryHandler implements IRequestHandler<GetContradictionAccountingQuery, spContradictionDebit[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetContradictionAccountingQuery): Promise<spContradictionDebit[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetContradictionAccountingQuery> = new HttpRequest<GetContradictionAccountingQuery>(request.url, request);

   
    if (request.CommodityId != undefined)
      httpRequest.Query += `CommodityId=${request.CommodityId}`
    if (request.accountHeadId != undefined)
      httpRequest.Query += `&accountHeadId=${request.accountHeadId}`;
    if (request.fromDate != undefined) {
      httpRequest.Query += `&fromDate=${request.fromDate?.toUTCString()}`;
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `&toDate=${request.toDate?.toUTCString()}`;
    }
    
    
      return await this._httpService.Post<GetContradictionAccountingQuery, ServiceResult<spContradictionDebit[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
