import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {MoadianInvoiceDetail} from "../../../../entities/moadian-invoice-detail";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";

export class CreateMoadianInvoiceDetailCommand extends IRequest<CreateMoadianInvoiceDetailCommand, MoadianInvoiceDetail> {

  public invoiceHeaderId: number | undefined = undefined;
  public sstid: string | undefined = undefined;
  public sstt: string | undefined = undefined;
  public mu: string | undefined = undefined;
  public am: number | undefined = undefined;
  public fee: number | undefined = undefined;
  public cfee: number | undefined = undefined;
  public cut: string | undefined = undefined;
  public exr: number | undefined = undefined;
  public prdis: number | undefined = undefined;
  public dis: number | undefined = undefined;
  public adis: number | undefined = undefined;
  public vra: number | undefined = undefined;
  public vam: number | undefined = undefined;
  public odt: string | undefined = undefined;
  public odr: number | undefined = undefined;
  public odam: number | undefined = undefined;
  public olt: string | undefined = undefined;
  public olr: number | undefined = undefined;
  public olam: number | undefined = undefined;
  public consfee: number | undefined = undefined;
  public spro: number | undefined = undefined;
  public bros: number | undefined = undefined;
  public tcpbs: number | undefined = undefined;
  public cop: number | undefined = undefined;
  public vop: number | undefined = undefined;
  public bsrn: string | undefined = undefined;
  public tsstam: number | undefined = undefined;
  public nw: number | undefined = undefined;
  public sscv: number | undefined = undefined;
  public ssrv: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: MoadianInvoiceDetail): CreateMoadianInvoiceDetailCommand {
    throw new ApplicationError(CreateMoadianInvoiceDetailCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MoadianInvoiceDetail {
    throw new ApplicationError(CreateMoadianInvoiceDetailCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateMoadianInvoiceDetailCommandHandler.name)
export class CreateMoadianInvoiceDetailCommandHandler implements IRequestHandler<CreateMoadianInvoiceDetailCommand, MoadianInvoiceDetail> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: CreateMoadianInvoiceDetailCommand): Promise<MoadianInvoiceDetail> {
    let httpRequest: HttpRequest<CreateMoadianInvoiceDetailCommand> = new HttpRequest<CreateMoadianInvoiceDetailCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateMoadianInvoiceDetailCommand, ServiceResult<MoadianInvoiceDetail>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
