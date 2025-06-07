import { Inject } from "@angular/core";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ReportFormatTypes } from "../ReportFormatTypes";
import { AccountReviewReportResultModel } from "../../account-review/account-review-report-result-model";
import { ServiceResult } from "src/app/core/models/service-result";

export class GetAccountReviewReportQuery extends IRequest<GetAccountReviewReportQuery, AccountReviewReportResultModel[]> {

  public reportType: number | undefined = undefined;
  public level: number | undefined = undefined;
  public accountReferencesGroupflag: number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public yearIds: any[] = [];
  public voucherStateId: number | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public transferId: number | undefined = undefined;
  public accountHeadIds: number[] = [];
  public referenceGroupIds: [] = [];
  public referenceIds: [] = [];
  public codeVoucherGroupIds: [] = [];
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
  public currencyTypeBaseId: number | undefined = undefined;

  constructor() {
    super();
  }
  mapFromSelf(query: GetAccountReviewReportQuery) {
    this.mapBasics(query, this);
    return this;
  }

  mapFrom(entity: AccountReviewReportResultModel[]): GetAccountReviewReportQuery {
    throw new ApplicationError(GetAccountReviewReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccountReviewReportResultModel[] {
    throw new ApplicationError(GetAccountReviewReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/Reporting/GetAccountReview";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountReviewReportQueryHandler.name)
export class GetAccountReviewReportQueryHandler implements IRequestHandler<GetAccountReviewReportQuery, AccountReviewReportResultModel[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetAccountReviewReportQuery): Promise<AccountReviewReportResultModel[]> {
    let httpRequest: HttpRequest<GetAccountReviewReportQuery> = new HttpRequest<GetAccountReviewReportQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()
    if (request.reportFormat != 1) httpRequest.ResponseType = 'blob';
    return await this._httpService.Post<GetAccountReviewReportQuery, ServiceResult<AccountReviewReportResultModel[]>>(httpRequest).toPromise().then(response => {
      //return request.reportFormat != 1 ? response : response
      return response.objResult.map(x => {
        x.accountReferencesGroupsTitle = [x.accountReferencesGroupsCode,x.accountReferencesGroupsTitle].join(' - ')
        return x;
      });
    })
  }
}
