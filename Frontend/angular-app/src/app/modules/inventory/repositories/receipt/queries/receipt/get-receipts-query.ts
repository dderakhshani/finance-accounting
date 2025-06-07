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



export class GetReceiptsQuery extends IRequest<GetReceiptsQuery, PaginatedList<Receipt>> {

  constructor(
    public documentStauseBaseValues: number,
    public isImportPurchase: boolean | null = null,
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Receipt>): GetReceiptsQuery {
    throw new ApplicationError(GetReceiptsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Receipt> {
    throw new ApplicationError(GetReceiptsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetReceiptsQueryHandler.name)
export class GetReceiptsQueryHandler implements IRequestHandler<GetReceiptsQuery, PaginatedList<Receipt>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetReceiptsQuery): Promise<PaginatedList<Receipt>> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<GetReceiptsQuery> = new HttpRequest<GetReceiptsQuery>(request.url, request);

    httpRequest.Query += `documentStauseBaseValue=${request.documentStauseBaseValues}&IsImportPurchase=${request.isImportPurchase != undefined ? request.isImportPurchase : _null}&fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;

    return await this._httpService.Post<GetReceiptsQuery, ServiceResult<PaginatedList<Receipt>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
