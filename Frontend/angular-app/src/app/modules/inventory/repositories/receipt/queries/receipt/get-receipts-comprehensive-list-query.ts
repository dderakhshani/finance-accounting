import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { Receipt } from "../../../../entities/receipt";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../../core/models/paginated-list";

import { PagesCommonService } from "../../../../../../shared/services/pages/pages-common.service";



export class GetComprehensiveListQuery extends IRequest<GetComprehensiveListQuery, PaginatedList<Receipt>> {

  constructor(
   
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Receipt>): GetComprehensiveListQuery {
    throw new ApplicationError(GetComprehensiveListQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Receipt> {
    throw new ApplicationError(GetComprehensiveListQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetComprehensiveList";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetComprehensiveListQueryHandler.name)
export class GetComprehensiveListQueryHandler implements IRequestHandler<GetComprehensiveListQuery, PaginatedList<Receipt>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetComprehensiveListQuery): Promise<PaginatedList<Receipt>> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<GetComprehensiveListQuery> = new HttpRequest<GetComprehensiveListQuery>(request.url, request);

    httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;

    return await this._httpService.Post<GetComprehensiveListQuery, ServiceResult<PaginatedList<Receipt>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
