import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ReportFormatTypes} from "../ReportFormatTypes";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {VoucherDetailReportResultModel} from "../../../entities/voucher-detail-report-result-model";

export class VoucherDetailsReportQuery extends IRequest<VoucherDetailsReportQuery, PaginatedList<VoucherDetailReportResultModel>> {

  public fromDate: Date | undefined = undefined;
  public toDate: Date | undefined = undefined;
  public groupingKeys: string[] = []
  public showLastLevelDetails: boolean = true;

  constructor(pageIndex: number = 0, pageSize: number = 100, searchQueries?: SearchQuery[], orderByProperty?: string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }

  mapFrom(entity: any): VoucherDetailsReportQuery {
    throw new ApplicationError(VoucherDetailsReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(VoucherDetailsReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/reporting/voucherDetails";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(VoucherDetailsReportQueryHandler.name)
export class VoucherDetailsReportQueryHandler implements IRequestHandler<VoucherDetailsReportQuery, PaginatedList<VoucherDetailReportResultModel>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: VoucherDetailsReportQuery): Promise<PaginatedList<VoucherDetailReportResultModel>> {
    let httpRequest: HttpRequest<VoucherDetailsReportQuery> = new HttpRequest<VoucherDetailsReportQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<VoucherDetailsReportQuery, ServiceResult<PaginatedList<VoucherDetailReportResultModel>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
