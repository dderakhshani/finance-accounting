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



export class GetAllReceiptItemsCommodityQuery extends IRequest<GetAllReceiptItemsCommodityQuery, PaginatedList<Receipt>> {

  constructor(
    public documentStauseBaseValue: number = 0,
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Receipt>): GetAllReceiptItemsCommodityQuery {
    throw new ApplicationError(GetAllReceiptItemsCommodityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Receipt> {
    throw new ApplicationError(GetAllReceiptItemsCommodityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetAllReceiptItemsCommodity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetAllReceiptItemsCommodityQueryHandler.name)
export class GetAllReceiptItemsCommodityQueryHandler implements IRequestHandler<GetAllReceiptItemsCommodityQuery, PaginatedList<Receipt>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetAllReceiptItemsCommodityQuery): Promise<PaginatedList<Receipt>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetAllReceiptItemsCommodityQuery> = new HttpRequest<GetAllReceiptItemsCommodityQuery>(request.url, request);


    httpRequest.Query += `documentStauseBaseValue=${request.documentStauseBaseValue}`;
    if (request.fromDate != undefined) {
      httpRequest.Query += `&fromDate=${request.fromDate?.toUTCString()}`;
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `&toDate=${request.toDate?.toUTCString()}`;
    }
    return await this._httpService.Post<GetAllReceiptItemsCommodityQuery, ServiceResult<PaginatedList<Receipt>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
