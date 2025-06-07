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
import { WarehouseLayoutsCommodity } from "../../../../entities/warehouse-layouts-commodity";



export class GetWarhouseLayoutHistotyByDocumentQuery extends IRequest<GetWarhouseLayoutHistotyByDocumentQuery, PaginatedList<Receipt>> {

  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Receipt>): GetWarhouseLayoutHistotyByDocumentQuery {
    throw new ApplicationError(GetWarhouseLayoutHistotyByDocumentQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Receipt> {
    throw new ApplicationError(GetWarhouseLayoutHistotyByDocumentQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayoutReport/GetAllHistoriesDocument";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetWarhouseLayoutHistotyByDocumentQueryHandler.name)
export class GetWarhouseLayoutHistotyByDocumentQueryHandler implements IRequestHandler<GetWarhouseLayoutHistotyByDocumentQuery, PaginatedList<Receipt>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetWarhouseLayoutHistotyByDocumentQuery): Promise<PaginatedList<Receipt>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarhouseLayoutHistotyByDocumentQuery> = new HttpRequest<GetWarhouseLayoutHistotyByDocumentQuery>(request.url, request);


    httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString() }`;

    return await this._httpService.Post<GetWarhouseLayoutHistotyByDocumentQuery, ServiceResult<PaginatedList<Receipt>>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult
    })

  }

}
