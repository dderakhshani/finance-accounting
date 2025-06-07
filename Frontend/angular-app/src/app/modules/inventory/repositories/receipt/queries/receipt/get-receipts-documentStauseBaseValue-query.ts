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



export class GetReceiptsdocumentStauseBaseValueQuery extends IRequest<GetReceiptsdocumentStauseBaseValueQuery, PaginatedList<Receipt>> {

  constructor(public documentStauseBaseValue: number,
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Receipt>): GetReceiptsdocumentStauseBaseValueQuery {
    throw new ApplicationError(GetReceiptsdocumentStauseBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Receipt> {
    throw new ApplicationError(GetReceiptsdocumentStauseBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetByDocumentStauseBaseValue";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetReceiptsdocumentStauseBaseValueQueryHandler.name)
export class GetReceiptsdocumentStauseBaseValueQueryHandler implements IRequestHandler<GetReceiptsdocumentStauseBaseValueQuery, PaginatedList<Receipt>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetReceiptsdocumentStauseBaseValueQuery): Promise<PaginatedList<Receipt>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetReceiptsdocumentStauseBaseValueQuery> = new HttpRequest<GetReceiptsdocumentStauseBaseValueQuery>(request.url, request);


    httpRequest.Query += `documentStauseBaseValue=${request.documentStauseBaseValue}&fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;

    return await this._httpService.Post<GetReceiptsdocumentStauseBaseValueQuery, ServiceResult<PaginatedList<Receipt>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
