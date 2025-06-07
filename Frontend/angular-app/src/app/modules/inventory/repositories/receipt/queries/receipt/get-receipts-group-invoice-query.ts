import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
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




export class GetReceiptGroupInvoiceQuery extends IRequest<GetReceiptGroupInvoiceQuery, PaginatedList<Receipt>> {

  constructor(
   
   
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public documentIds: string | undefined = undefined,
    public VoucherHeadId: number | undefined = undefined,
    public creditAccountReferenceId : number | undefined = undefined,
    public debitAccountReferenceId: number | undefined = undefined,
    public creditAccountHeadId: number | undefined = undefined,
    public debitAccountHeadId: number | undefined = undefined,
    
    public creditAccountReferenceGroupId: number | undefined = undefined,
    public debitAccountReferenceGroupId: number | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Receipt>): GetReceiptGroupInvoiceQuery {
    throw new ApplicationError(GetReceiptGroupInvoiceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Receipt> {
    throw new ApplicationError(GetReceiptGroupInvoiceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetReceiptGroupInvoice";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetReceiptGroupInvoiceQueryHandler.name)
export class GetReceiptGroupInvoiceQueryHandler implements IRequestHandler<GetReceiptGroupInvoiceQuery, PaginatedList<Receipt>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetReceiptGroupInvoiceQuery): Promise<PaginatedList<Receipt>> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<GetReceiptGroupInvoiceQuery> = new HttpRequest<GetReceiptGroupInvoiceQuery>(request.url, request);

    
    if (request.fromDate != undefined) {
      httpRequest.Query += `fromDate=${request.fromDate?.toUTCString() }&`
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `toDate=${request.toDate?.toUTCString() }&`
    }
    if (request.documentIds != undefined) {
      httpRequest.Query += `documentIds=${request.documentIds}&`
    }
    if (request.creditAccountReferenceId != undefined) {
      httpRequest.Query += `creditAccountReferenceId=${request.creditAccountReferenceId}&`
    }
    if (request.debitAccountReferenceId != undefined) {
      httpRequest.Query += `debitAccountReferenceId=${request.debitAccountReferenceId}&`
    }
    if (request.creditAccountHeadId != undefined) {
      httpRequest.Query += `creditAccountHeadId=${request.creditAccountHeadId}&`
    }
    if (request.creditAccountReferenceGroupId != undefined) {
      httpRequest.Query += `creditAccountReferenceGroupId=${request.creditAccountReferenceGroupId}&`
    }
    if (request.debitAccountReferenceGroupId != undefined) {
      httpRequest.Query += `debitAccountReferenceGroupId=${request.debitAccountReferenceGroupId}&`
    }
    if (request.debitAccountHeadId != undefined) {
      httpRequest.Query += `debitAccountHeadId=${request.debitAccountHeadId}&`
    }
    if (request.VoucherHeadId != undefined) {
      httpRequest.Query += `VoucherHeadId=${request.VoucherHeadId}&`
    }
    
    return await this._httpService.Post<GetReceiptGroupInvoiceQuery, ServiceResult<PaginatedList<Receipt>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
