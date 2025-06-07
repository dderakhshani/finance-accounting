import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {MoadianInvoiceHeader} from "../../../../entities/moadian-invoice-header";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {
  CreateMoadianInvoiceDetailCommand
} from "../../moadian-invoice-detail/commands/create-moadian-invoice-detail-command";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";

export class CreateMoadianInvoiceHeaderCommand extends IRequest<CreateMoadianInvoiceHeaderCommand, MoadianInvoiceHeader> {

  public isSandbox: boolean | undefined = undefined;
  public personFullName: string | undefined = undefined;
  public accountReferenceCode: string | undefined = undefined;
  public customerCode: string | undefined = undefined;

  public errors: string | undefined = undefined;
  public status: string | undefined = undefined;
  public referenceId: string | undefined = undefined;
  public submissionDate: Date | undefined = undefined;

  public taxId: string | undefined = undefined;
  public indatim: number | undefined = undefined;
  public indati2m: number | undefined = undefined;
  public inty: number | undefined = undefined;
  public inno: string | undefined = undefined;
  public invoiceNumber: string | undefined = undefined;
  public irtaxid: string | undefined = undefined;
  public inp: number | undefined = undefined;
  public ins: number | undefined = undefined;
  public tins: string | undefined = undefined;
  public tob: number | undefined = undefined;
  public bid: string | undefined = undefined;
  public tinb: string | undefined = undefined;
  public sbc: string | undefined = undefined;
  public bpc: string | undefined = undefined;
  public bbc: string | undefined = undefined;
  public ft: number | undefined = undefined;
  public bpn: string | undefined = undefined;
  public scln: string | undefined = undefined;
  public scc: string | undefined = undefined;
  public crn: string | undefined = undefined;
  public billid: string | undefined = undefined;
  public tprdis: number | undefined = undefined;
  public tdis: number | undefined = undefined;
  public tadis: number | undefined = undefined;
  public tvam: number | undefined = undefined;
  public todam: number | undefined = undefined;
  public tbill: number | undefined = undefined;
  public setm: number | undefined = undefined;
  public cap: number | undefined = undefined;
  public insp: number | undefined = undefined;
  public tvop: number | undefined = undefined;
  public tax17: number | undefined = undefined;
  public cdcn: string | undefined = undefined;
  public cdcd: number | undefined = undefined;
  public tonw: number | undefined = undefined;
  public torv: number | undefined = undefined;
  public tocv: number | undefined = undefined;

  public moadianInvoiceDetails: CreateMoadianInvoiceDetailCommand[] = []

  constructor() {
    super();
  }

  mapFrom(entity: MoadianInvoiceHeader): CreateMoadianInvoiceHeaderCommand {
    throw new ApplicationError(CreateMoadianInvoiceHeaderCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MoadianInvoiceHeader {
    throw new ApplicationError(CreateMoadianInvoiceHeaderCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/moadian/addInvoice";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateMoadianInvoiceHeaderCommandHandler.name)
export class CreateMoadianInvoiceHeaderCommandHandler implements IRequestHandler<CreateMoadianInvoiceHeaderCommand, MoadianInvoiceHeader> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateMoadianInvoiceHeaderCommand): Promise<MoadianInvoiceHeader> {
    let httpRequest: HttpRequest<CreateMoadianInvoiceHeaderCommand> = new HttpRequest<CreateMoadianInvoiceHeaderCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateMoadianInvoiceHeaderCommand, ServiceResult<MoadianInvoiceHeader>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
