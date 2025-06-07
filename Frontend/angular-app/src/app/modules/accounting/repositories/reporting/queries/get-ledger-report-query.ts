import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ReportFormatTypes } from "../ReportFormatTypes";

export class GetLedgerReportQuery extends IRequest<GetLedgerReportQuery, any> {


  public useEF: boolean = true;
  public reportType: number | undefined = undefined;
  public level: number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public yearIds: number[] = [];
  public voucherStateId: number | undefined = undefined;
  public codeVoucherGroupIds: number[] = [];
  public transferId: number | undefined = undefined;
  public accountHeadIds: number[] = [];
  public referenceGroupIds: number[] = [];
  public referenceIds: number[] = [];
  public referenceNo: number | undefined = undefined;
  public voucherNoFrom: number | undefined = undefined;
  public voucherNoTo: number | undefined = undefined;
  public voucherDateFrom: Date | undefined = undefined;
  public voucherDateTo: Date | undefined = undefined;
  public debitFrom: number | undefined = undefined;
  public debitTo: number | undefined = undefined;
  public creditFrom: number | undefined = undefined;
  public creditTo: number | undefined = undefined;
  public documentIdFrom: number | undefined = undefined;
  public documentIdTo: number | undefined = undefined;
  public voucherDescription: string | undefined = undefined;
  public voucherRowDescription: string | undefined = undefined;
  public reportTitle: string | undefined = undefined;
  public remain: boolean | undefined = undefined;
  public reportFormat: ReportFormatTypes | undefined = undefined;
  public isPrint: boolean | undefined = undefined;
  public printType: Number | undefined = undefined;
  public forcePrint: boolean | undefined = undefined;
  public chequeSheetIds: number[] = [];
  public currencyTypeBaseId: number | undefined = undefined;
  public usePagination: boolean | undefined;

  constructor() {
    super();
  }


  mapFrom(entity: any): GetLedgerReportQuery {
    throw new ApplicationError(GetLedgerReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(GetLedgerReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/reporting/getReportLedger";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetLedgerReportQueryHandler.name)
export class GetLedgerReportQueryHandler implements IRequestHandler<GetLedgerReportQuery, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetLedgerReportQuery): Promise<any> {
    let httpRequest: HttpRequest<GetLedgerReportQuery> = new HttpRequest<GetLedgerReportQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetLedgerReportQuery, ServiceResult<any>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
