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
import { PaginatedList } from "../../../../core/models/paginated-list";
import { spErpDarkhastJoinDocumentHeads } from "../../entities/spErpDarkhastJoinDocumentHeads";

export class ErpDarkhastJoinDocumentHeadsQuery extends IRequest<ErpDarkhastJoinDocumentHeadsQuery,  PaginatedList<spErpDarkhastJoinDocumentHeads>> {
  constructor(
    
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public requestNo: string | undefined = undefined,
   
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity:  PaginatedList<spErpDarkhastJoinDocumentHeads>): ErpDarkhastJoinDocumentHeadsQuery {
    throw new ApplicationError(ErpDarkhastJoinDocumentHeadsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo():  PaginatedList<spErpDarkhastJoinDocumentHeads> {
    throw new ApplicationError(ErpDarkhastJoinDocumentHeadsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/AraniRequest/GetErpDarkhastJoinDocumentHeads";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ErpDarkhastJoinDocumentHeadsQueryHandler.name)
export class ErpDarkhastJoinDocumentHeadsQueryHandler implements IRequestHandler<ErpDarkhastJoinDocumentHeadsQuery,  PaginatedList<spErpDarkhastJoinDocumentHeads>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: ErpDarkhastJoinDocumentHeadsQuery): Promise< PaginatedList<spErpDarkhastJoinDocumentHeads>> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<ErpDarkhastJoinDocumentHeadsQuery> = new HttpRequest<ErpDarkhastJoinDocumentHeadsQuery>(request.url, request);

    httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;
    if (request.requestNo != undefined) {
      httpRequest.Query += `&requestNo=${request.requestNo}`
    }
    
    return await this._httpService.Post<ErpDarkhastJoinDocumentHeadsQuery, ServiceResult< PaginatedList<spErpDarkhastJoinDocumentHeads>>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
