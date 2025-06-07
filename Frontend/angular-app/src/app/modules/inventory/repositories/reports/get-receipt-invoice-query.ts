import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { Receipt } from "../../entities/receipt";
import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { PagesCommonService } from "../../../../shared/services/pages/pages-common.service";

export class GetRecepitInvoiceQuery extends IRequest<GetRecepitInvoiceQuery, Receipt[]> {
  constructor(
    
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public invoiceNo: string | undefined = undefined,
    public CreditAccountReferenceId: string | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: Receipt[]): GetRecepitInvoiceQuery {
    throw new ApplicationError(GetRecepitInvoiceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt[] {
    throw new ApplicationError(GetRecepitInvoiceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetAllInvoice";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetRecepitInvoiceQueryHandler.name)
export class GetRecepitInvoiceQueryHandler implements IRequestHandler<GetRecepitInvoiceQuery, Receipt[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetRecepitInvoiceQuery): Promise<Receipt[]> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<GetRecepitInvoiceQuery> = new HttpRequest<GetRecepitInvoiceQuery>(request.url, request);

    httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;
    if (request.invoiceNo != undefined) {
      httpRequest.Query += `&InvoiceNo=${request.invoiceNo}`
    }
    if (request.CreditAccountReferenceId != undefined) {
      httpRequest.Query += `&CreditAccountReferenceId=${request.CreditAccountReferenceId}`
    }
    
    
    return await this._httpService.Post<GetRecepitInvoiceQuery, ServiceResult<Receipt[]>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
