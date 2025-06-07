import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { invoice } from "../../../../entities/invoice";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../../core/models/paginated-list";
import { Data } from "@angular/router";
import { PagesCommonService } from "../../../../../../shared/services/pages/pages-common.service";




export class GetInvoiceActiveRequestNo extends IRequest<GetInvoiceActiveRequestNo, PaginatedList<invoice>> {

  constructor(
    public requestNo: number,
    public referenceId: number,
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<invoice>): GetInvoiceActiveRequestNo {
    throw new ApplicationError(GetInvoiceActiveRequestNo.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<invoice> {
    throw new ApplicationError(GetInvoiceActiveRequestNo.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/invoice/GetInvoiceActiveRequestNo";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetInvoiceActiveRequestNoHandler.name)
export class GetInvoiceActiveRequestNoHandler implements IRequestHandler<GetInvoiceActiveRequestNo, PaginatedList<invoice>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetInvoiceActiveRequestNo): Promise<PaginatedList<invoice>> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetInvoiceActiveRequestNo> = new HttpRequest<GetInvoiceActiveRequestNo>(request.url, request);
    httpRequest.Query += `RequestNo=${request.requestNo}&referenceId=${request.referenceId}`;
    httpRequest.Query += request.fromDate ? `&fromDate=${request.fromDate?.toUTCString()}` : '';
    httpRequest.Query += request.toDate ? `&toDate=${request.toDate?.toUTCString()}` : '';

    return await this._httpService.Post<GetInvoiceActiveRequestNo, ServiceResult<PaginatedList<invoice>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }

}
