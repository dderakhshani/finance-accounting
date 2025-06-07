import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../core/validation/validation-rule";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {ApplicationError} from "../../../../core/exceptions/application-error";
import {RequestLog} from "./request-log";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {ServiceResult} from "../../../../core/models/service-result";
import {PaginatedList} from "../../../../core/models/paginated-list";

export class GetApplicationRequestLogsQuery extends IRequest<GetApplicationRequestLogsQuery, PaginatedList<RequestLog>>{
  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: any): GetApplicationRequestLogsQuery {
    throw new ApplicationError(GetApplicationRequestLogsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(GetApplicationRequestLogsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/logs/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
@MediatorHandler(GetApplicationRequestLogsQueryHandler.name)
export class GetApplicationRequestLogsQueryHandler implements IRequestHandler<GetApplicationRequestLogsQuery, PaginatedList<RequestLog>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) { }

  async Handle(request: GetApplicationRequestLogsQuery): Promise<PaginatedList<RequestLog>> {
    let httpRequest = new HttpRequest<GetApplicationRequestLogsQuery>(request.url, request);

    return await this._httpService.Post<any,ServiceResult<PaginatedList<RequestLog>>>(httpRequest).toPromise().then(res => {
      return res.objResult;
    });

  }
}

