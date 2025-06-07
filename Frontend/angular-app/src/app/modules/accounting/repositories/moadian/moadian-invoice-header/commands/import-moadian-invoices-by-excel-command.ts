import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {MoadianInvoiceHeader} from "../../../../entities/moadian-invoice-header";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";

export class ImportMoadianInvoicesByExcelCommand extends IRequest<ImportMoadianInvoicesByExcelCommand, MoadianInvoiceHeader[]> {
  public file: File | undefined = undefined;
  public isProduction: boolean | undefined = undefined;
  public generateTaxIdWithListNumber: boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: MoadianInvoiceHeader[]): ImportMoadianInvoicesByExcelCommand {
    throw new ApplicationError(ImportMoadianInvoicesByExcelCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MoadianInvoiceHeader[] {
    throw new ApplicationError(ImportMoadianInvoicesByExcelCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/Moadian/ImportInvoicesByExcel";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ImportMoadianInvoicesByExcelCommandHandler.name)
export class ImportMoadianInvoicesByExcelCommandHandler implements IRequestHandler<ImportMoadianInvoicesByExcelCommand, MoadianInvoiceHeader[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: ImportMoadianInvoicesByExcelCommand): Promise<MoadianInvoiceHeader[]> {
    let httpRequest: HttpRequest<ImportMoadianInvoicesByExcelCommand> = new HttpRequest<ImportMoadianInvoicesByExcelCommand>(request.url, request);
    httpRequest.BodyFormat = 'form';
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<ImportMoadianInvoicesByExcelCommand, ServiceResult<MoadianInvoiceHeader[]>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
