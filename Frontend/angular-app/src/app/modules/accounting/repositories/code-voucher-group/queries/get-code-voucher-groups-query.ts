import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CodeVoucherGroup} from "../../../entities/code-voucher-group";
import {PaginatedList} from "../../../../../core/models/paginated-list";


export class GetCodeVoucherGroupsQuery extends IRequest<GetCodeVoucherGroupsQuery, PaginatedList<CodeVoucherGroup>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<CodeVoucherGroup>): GetCodeVoucherGroupsQuery {
    throw new ApplicationError(GetCodeVoucherGroupsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<CodeVoucherGroup> {
    throw new ApplicationError(GetCodeVoucherGroupsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherGroup/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCodeVoucherGroupsQueryHandler.name)
export class GetCodeVoucherGroupsQueryHandler implements IRequestHandler<GetCodeVoucherGroupsQuery, PaginatedList<CodeVoucherGroup>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: GetCodeVoucherGroupsQuery): Promise<PaginatedList<CodeVoucherGroup>> {
    let httpRequest: HttpRequest<GetCodeVoucherGroupsQuery> = new HttpRequest<GetCodeVoucherGroupsQuery>(request.url, request);

    return await this._httpService.Post<GetCodeVoucherGroupsQuery, ServiceResult<PaginatedList<CodeVoucherGroup>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
