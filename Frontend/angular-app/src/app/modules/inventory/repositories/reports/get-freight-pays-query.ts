import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { StpFreightPays } from "../../entities/StpFreightPays";
import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { PagesCommonService } from "../../../../shared/services/pages/pages-common.service";
import { PaginatedList } from "../../../../core/models/paginated-list";

export class GetFreightPaysQuery extends IRequest<GetFreightPaysQuery, PaginatedList<StpFreightPays>> {
  constructor(
    
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public accountReferenceId: string | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<StpFreightPays>): GetFreightPaysQuery {
    throw new ApplicationError(GetFreightPaysQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<StpFreightPays> {
    throw new ApplicationError(GetFreightPaysQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetFreightPays";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetFreightPaysQueryHandler.name)
export class GetFreightPaysQueryHandler implements IRequestHandler<GetFreightPaysQuery, PaginatedList<StpFreightPays>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetFreightPaysQuery): Promise<PaginatedList<StpFreightPays>> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<GetFreightPaysQuery> = new HttpRequest<GetFreightPaysQuery>(request.url, request);

    httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;
    
    if (request.accountReferenceId != undefined) {
      httpRequest.Query += `&AccountReferenceId=${request.accountReferenceId}`
    }
    
    
    return await this._httpService.Post<GetFreightPaysQuery, ServiceResult<PaginatedList<StpFreightPays>>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
