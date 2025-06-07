import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {invoice} from "../../../../entities/invoice";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../../core/models/paginated-list";
import {Data} from "@angular/router";
import {PagesCommonService} from "../../../../../../shared/services/pages/pages-common.service";


export class GetInvoiceActiveCommodityId extends IRequest<GetInvoiceActiveCommodityId, PaginatedList<invoice>> {

  constructor(
    public commodityId: number,
    public referenceId: number,
    public fromDate?: Date,
    public toDate?: Date,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<invoice>): GetInvoiceActiveCommodityId {
    throw new ApplicationError(GetInvoiceActiveCommodityId.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<invoice> {
    throw new ApplicationError(GetInvoiceActiveCommodityId.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/invoice/GetInvoiceActiveCommodityId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler(GetInvoiceActiveCommodityIdHandler.name)
export class GetInvoiceActiveCommodityIdHandler implements IRequestHandler<GetInvoiceActiveCommodityId, PaginatedList<invoice>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService
  ) {
  }

  async Handle(request: GetInvoiceActiveCommodityId): Promise<PaginatedList<invoice>> {
    this._notificationService.isLoaderDropdown = true;

    let httpRequest: HttpRequest<GetInvoiceActiveCommodityId> = new HttpRequest<GetInvoiceActiveCommodityId>(request.url, request);

    httpRequest.Query += `commodityId=${request.commodityId}&referenceId=${request.referenceId}`;
    httpRequest.Query += request.fromDate ? `&fromDate=${this.inventoryService.formatDate(request.fromDate) }` : '';
    ;
    httpRequest.Query += request.toDate ? `&toDate=${this.inventoryService.formatDate(request.toDate)}` : '';

    return await this._httpService.Post<GetInvoiceActiveCommodityId, ServiceResult<PaginatedList<invoice>>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }

}
