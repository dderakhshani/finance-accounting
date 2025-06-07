import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { Help } from "../../../entities/help";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { Inject } from "@angular/core";
import { HttpService } from "../../../../../core/services/http/http.service";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "../../../../../core/models/service-result";

export class GetHelpsQuery extends IRequest<GetHelpsQuery, PaginatedList<Help>>{

  constructor(
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '',
    public isMinify: boolean = false
  ) {
    super()
  }

  mapFrom(entity: PaginatedList<Help>): GetHelpsQuery {
    throw new ApplicationError(GetHelpsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Help> {
    throw new ApplicationError(GetHelpsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/help/getall"
  }

  get validationRules() : ValidationRule[]{
    return [];
  }
}

@MediatorHandler(GetHelpsQueryHandler.name)
export class GetHelpsQueryHandler implements  IRequestHandler<GetHelpsQuery, PaginatedList<Help>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetHelpsQuery): Promise<PaginatedList<Help>> {
    let httpRequest : HttpRequest<GetHelpsQuery> = new HttpRequest<GetHelpsQuery>(request.url, request);

    return await this._httpService.Post<GetHelpsQuery, ServiceResult<PaginatedList<Help>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
