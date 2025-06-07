import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ReportFormatTypes} from "../../../../accounting/repositories/reporting/ReportFormatTypes";

export class GetCentralBankReportQuery extends IRequest<GetCentralBankReportQuery, string> {
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


  mapFrom(entity: any): GetCentralBankReportQuery {
    this.mapBasics(entity, this)
    this.yearIds = entity.yearIds;
    this.accountHeadIds = entity.accountHeadIds
    this.referenceGroupIds = entity.referenceGroupIds
    this.referenceIds = entity.referenceIds
    this.codeVoucherGroupIds = entity.codeVoucherGroupIds
    return this;
  }

  mapTo(): string {
    throw new ApplicationError(GetCentralBankReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/Reporting/getcentralbankreport";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCentralBankReportQueryHandler.name)
export class GetCentralBankReportQueryHandler implements IRequestHandler<GetCentralBankReportQuery, string> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCentralBankReportQuery): Promise<string> {
    let httpRequest: HttpRequest<GetCentralBankReportQuery> = new HttpRequest<GetCentralBankReportQuery>(request.url, request);

    return await this._httpService.Post<GetCentralBankReportQuery, ServiceResult<string>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
