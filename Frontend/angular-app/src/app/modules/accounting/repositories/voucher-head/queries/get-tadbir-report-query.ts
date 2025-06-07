import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {VoucherHead} from "../../../entities/voucher-head";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";

export class GetTadbirReportQuery extends IRequest<GetTadbirReportQuery, any> {

  public VoucherHeadIds: number[] = []

  constructor(searchQueries?: SearchQuery[], orderByProperty?: string) {
    super();

    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }

  mapFrom(entity: any): GetTadbirReportQuery {
    throw new ApplicationError(GetTadbirReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(GetTadbirReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/Reporting/TadbirReport";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(GetTadbirReportQueryHandler.name)
export class GetTadbirReportQueryHandler implements IRequestHandler<GetTadbirReportQuery, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: GetTadbirReportQuery): Promise<any> {

    let httpRequest: HttpRequest<any> = new HttpRequest<any>(request.url, request);

    return await this._httpService.Post<any, ServiceResult<any>>(httpRequest).toPromise().then(res => {
      return res.objResult;
    });
  };
}
