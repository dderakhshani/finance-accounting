import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import Bom from "../../../entities/bom";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import { BomHeaderValue } from "../../../entities/bom-header-value";

export class GetBomHeadersAllQuery extends IRequest<GetBomHeadersAllQuery, PaginatedList<BomHeaderValue[]>> {

  constructor(number = 0,public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<any>): GetBomHeadersAllQuery {
    throw new ApplicationError(GetBomHeadersAllQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<any> {
    throw new ApplicationError(GetBomHeadersAllQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/BomValueHeader/GetAllBomValueHeaders";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBomHeadersAllQueryHandler.name)
export class GetBomHeadersAllQueryHandler implements IRequestHandler<GetBomHeadersAllQuery, PaginatedList<BomHeaderValue[]>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetBomHeadersAllQuery): Promise<PaginatedList<BomHeaderValue[]>> {
    let httpRequest: HttpRequest<GetBomHeadersAllQuery> = new HttpRequest<GetBomHeadersAllQuery>(request.url, request);
   

    return await this._httpService.Post<GetBomHeadersAllQuery, ServiceResult<PaginatedList<BomHeaderValue[]>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    })

  }
}
