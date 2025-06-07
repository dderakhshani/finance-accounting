import {Inject} from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
import { ReceiptsExcelUploaded } from "src/app/modules/bursary/entities/receipts-excel-upload";
import { NotificationService } from "src/app/shared/services/notification/notification.service";


export class GetUploadedReceiptsQuery extends IRequest<GetUploadedReceiptsQuery, ReceiptsExcelUploaded> {



  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: ReceiptsExcelUploaded): GetUploadedReceiptsQuery {
    throw new ApplicationError(GetUploadedReceiptsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ReceiptsExcelUploaded {
    throw new ApplicationError(GetUploadedReceiptsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/CustomerReceipt/GetUploadedReceipts";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetReceiptQueryHandler.name)
export class GetReceiptQueryHandler implements IRequestHandler<GetUploadedReceiptsQuery, ReceiptsExcelUploaded> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetUploadedReceiptsQuery): Promise<ReceiptsExcelUploaded> {
    let httpRequest: HttpRequest<GetUploadedReceiptsQuery> = new HttpRequest<GetUploadedReceiptsQuery>(request.url, request);
    return await this._httpService.Get<ReceiptsExcelUploaded>(httpRequest).toPromise().then(response => {
      return response
    })
  }
}
