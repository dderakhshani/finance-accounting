import {ReportFormatTypes} from "../ReportFormatTypes";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetBalanceReportQuery extends IRequest<GetBalanceReportQuery, any[]> {

  public reportType: number | undefined = undefined;
  public level: number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public yearIds: [] = [];
  public voucherStateId: number | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public transferId: number | undefined = undefined;
  public accountHeadIds: [] = [];
  public referenceGroupIds: [] = [];
  public referenceIds: [] = [];
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

  constructor() {
    super();
  }


  mapFrom(entity: any[]): GetBalanceReportQuery {
    throw new ApplicationError(GetBalanceReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any[] {
    throw new ApplicationError(GetBalanceReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/reporting/getReportBalance6";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBalanceReportQueryHandler.name)
export class GetBalanceReportQueryHandler implements IRequestHandler<GetBalanceReportQuery, any[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetBalanceReportQuery): Promise<any[]> {
    let httpRequest: HttpRequest<GetBalanceReportQuery> = new HttpRequest<GetBalanceReportQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetBalanceReportQuery, ServiceResult<any[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
