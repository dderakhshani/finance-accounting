import {Inject} from "@angular/core";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "src/app/core/models/service-result";

export class GetSSRSReportURLQuery extends IRequest<GetSSRSReportURLQuery, string> {

  constructor(public reportName: string) {
    super();
  }

  mapFromSelf(query: GetSSRSReportURLQuery) {
    this.mapBasics(query, this);
    return this;
  }

  mapFrom(entity: string): GetSSRSReportURLQuery {
    throw new ApplicationError(GetSSRSReportURLQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): string {
    throw new ApplicationError(GetSSRSReportURLQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/Reporting/GetSSRSReportAddress";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetSSRSReportURLQueryHandler.name)
export class GetSSRSReportURLQueryHandler implements IRequestHandler<GetSSRSReportURLQuery, string> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetSSRSReportURLQuery): Promise<string> {
    let httpRequest: HttpRequest<GetSSRSReportURLQuery> = new HttpRequest<GetSSRSReportURLQuery>(request.url, request);
    httpRequest.Query += `?reportName=${request.reportName}`;

    return await this._httpService.Get<ServiceResult<string>>(httpRequest).toPromise().then(response => {
      return response.objResult;
    })
  }
}
