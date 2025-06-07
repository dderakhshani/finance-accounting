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
import { BomHeader } from "../../../entities/boms-header";
import { BomItem } from "../../../entities/bom-Item";

export class GetBomHeadersQuery extends IRequest<GetBomHeadersQuery, PaginatedList<BomHeader[]>> {

  constructor(public commodityId: number = 0,public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<any>): GetBomHeadersQuery {
    throw new ApplicationError(GetBomHeadersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<any> {
    throw new ApplicationError(GetBomHeadersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/BomValueHeader/GetAllByCommodityId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBomHeadersQueryHandler.name)
export class GetBomHeadersQueryHandler implements IRequestHandler<GetBomHeadersQuery, PaginatedList<BomHeader[]>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetBomHeadersQuery): Promise<PaginatedList<BomHeader[]>> {
    let httpRequest: HttpRequest<GetBomHeadersQuery> = new HttpRequest<GetBomHeadersQuery>(request.url, request);
    httpRequest.Query += `commodityId=${request.commodityId}`;

    return await this._httpService.Post<GetBomHeadersQuery, ServiceResult<PaginatedList<BomHeader[]>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    })

  }
}
