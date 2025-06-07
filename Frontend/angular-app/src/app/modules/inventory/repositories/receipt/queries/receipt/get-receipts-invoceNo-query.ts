
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



export class GetReceiptsByInvoiceNoQuery extends IRequest<GetReceiptsByInvoiceNoQuery, Receipt> {


  constructor(

    public invoiceNo: string,
    public fromDate: Date | undefined = undefined,
    public creditAccountReferenceId :number| undefined = undefined,
    ) {
    super();
  }


  mapFrom(entity: Receipt): GetReceiptsByInvoiceNoQuery {
    throw new ApplicationError(GetReceiptsByInvoiceNoQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetReceiptsByInvoiceNoQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetByInvoiceNo";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetReceiptsByInvoiceNoQueryHandler.name)
export class GetReceiptsByInvoiceNoQueryHandler implements IRequestHandler<GetReceiptsByInvoiceNoQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: GetReceiptsByInvoiceNoQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetReceiptsByInvoiceNoQuery> = new HttpRequest<GetReceiptsByInvoiceNoQuery>(request.url, request);

    if (request.fromDate != undefined) {
      httpRequest.Query += `date=${request.fromDate}&`
    }
    if (request.invoiceNo != undefined) {
      httpRequest.Query += `invoiceNo=${request.invoiceNo}&`
    }
    if (request.creditAccountReferenceId != undefined) {
      httpRequest.Query += `creditAccountReferenceId=${request.creditAccountReferenceId}`
    }
   
    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {


      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
