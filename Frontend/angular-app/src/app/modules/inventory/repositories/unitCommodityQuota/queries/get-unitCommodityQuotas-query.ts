import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import { PagesCommonService } from "../../../../../shared/services/pages/pages-common.service";
import { UnitCommodityQuota } from "../../../entities/unitCommodityQuota";


export class GetUnitCommodityQuotasQuery extends IRequest<GetUnitCommodityQuotasQuery, PaginatedList<UnitCommodityQuota>> {

  constructor(
    //public fromDate: Date | undefined = undefined,
    //public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<UnitCommodityQuota>): GetUnitCommodityQuotasQuery {
    throw new ApplicationError(GetUnitCommodityQuotasQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<UnitCommodityQuota> {
    throw new ApplicationError(GetUnitCommodityQuotasQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/UnitCommodityQuota/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetUnitCommodityQuotasQueryHandler.name)
export class GetUnitCommodityQuotasQueryHandler implements IRequestHandler<GetUnitCommodityQuotasQuery, PaginatedList<UnitCommodityQuota>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService
  ) {
  }

  async Handle(request: GetUnitCommodityQuotasQuery): Promise<PaginatedList<UnitCommodityQuota>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetUnitCommodityQuotasQuery> = new HttpRequest<GetUnitCommodityQuotasQuery>(request.url, request);


    /*httpRequest.Query += `fromDate=${request.fromDate}&toDate=${request.toDate}`;*/

    return await this._httpService.Post<GetUnitCommodityQuotasQuery, ServiceResult<PaginatedList<UnitCommodityQuota>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
