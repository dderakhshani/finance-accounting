import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";

export class SubmitMoadianInvoicesByIdsCommand extends IRequest<SubmitMoadianInvoicesByIdsCommand, any> {

  public code:string | undefined = undefined;
  constructor(public invoiceIds:number[] = [],public isProduction:boolean = false,public generateCode:boolean = false) {
    super();
  }

  mapFrom(entity: any): SubmitMoadianInvoicesByIdsCommand {
    throw new ApplicationError(SubmitMoadianInvoicesByIdsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(SubmitMoadianInvoicesByIdsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/Moadian/SubmitInvoices";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(SubmitMoadianInvoicesByIdsCommandHandler.name)
export class SubmitMoadianInvoicesByIdsCommandHandler implements IRequestHandler<SubmitMoadianInvoicesByIdsCommand, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService
  ) {
  }

  async Handle(request: SubmitMoadianInvoicesByIdsCommand): Promise<boolean> {
    let httpRequest: HttpRequest<SubmitMoadianInvoicesByIdsCommand> = new HttpRequest<SubmitMoadianInvoicesByIdsCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<SubmitMoadianInvoicesByIdsCommand, ServiceResult<any>>(httpRequest).toPromise().then(response => {
      if(request.code) this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
