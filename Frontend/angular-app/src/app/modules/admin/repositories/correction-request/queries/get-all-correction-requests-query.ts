import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CorrectionRequest} from "../../../entities/correction-request";

export class GetAllCorrectionRequestsQuery extends IRequest<GetAllCorrectionRequestsQuery, PaginatedList<CorrectionRequest>> {
  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<CorrectionRequest>): GetAllCorrectionRequestsQuery {
    throw new ApplicationError(GetAllCorrectionRequestsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<CorrectionRequest> {
    throw new ApplicationError(GetAllCorrectionRequestsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/correctionRequest/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAllCorrectionRequestsQueryHandler.name)
export class GetAllCorrectionRequestsQueryHandler implements IRequestHandler<GetAllCorrectionRequestsQuery, PaginatedList<CorrectionRequest>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetAllCorrectionRequestsQuery): Promise<PaginatedList<CorrectionRequest>> {
    let httpRequest: HttpRequest<GetAllCorrectionRequestsQuery> = new HttpRequest<GetAllCorrectionRequestsQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetAllCorrectionRequestsQuery, ServiceResult<PaginatedList<CorrectionRequest>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
