import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";

export class InquiryMoadianInvoicesByIdsCommand extends IRequest<InquiryMoadianInvoicesByIdsCommand, boolean> {

  constructor(public invoiceIds:number[],public isProduction:boolean) {
    super();
  }

  mapFrom(entity: boolean): InquiryMoadianInvoicesByIdsCommand {
    throw new ApplicationError(InquiryMoadianInvoicesByIdsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): boolean {
    throw new ApplicationError(InquiryMoadianInvoicesByIdsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/Moadian/InquiryInvoices";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(InquiryMoadianInvoicesByIdsCommandHandler.name)
export class InquiryMoadianInvoicesByIdsCommandHandler implements IRequestHandler<InquiryMoadianInvoicesByIdsCommand, boolean> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService
  ) {
  }

  async Handle(request: InquiryMoadianInvoicesByIdsCommand): Promise<boolean> {
    let httpRequest: HttpRequest<InquiryMoadianInvoicesByIdsCommand> = new HttpRequest<InquiryMoadianInvoicesByIdsCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<InquiryMoadianInvoicesByIdsCommand, ServiceResult<boolean>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
