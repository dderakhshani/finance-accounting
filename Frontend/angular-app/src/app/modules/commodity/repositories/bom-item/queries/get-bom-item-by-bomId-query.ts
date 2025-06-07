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

export class GetBomItemBybomIdQuery extends IRequest<GetBomItemBybomIdQuery, PaginatedList<any>> {

  constructor(public bomId: number = 0,public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<BomHeader>): GetBomItemBybomIdQuery {
    throw new ApplicationError(GetBomItemBybomIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<BomHeader> {
    throw new ApplicationError(GetBomItemBybomIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/BomItem/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBomItemBybomIdQueryHandler.name)
export class GetBomItemBybomIdQueryHandler implements IRequestHandler<GetBomItemBybomIdQuery, PaginatedList<any>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetBomItemBybomIdQuery): Promise<PaginatedList<any>> {
    let httpRequest: HttpRequest<GetBomItemBybomIdQuery> = new HttpRequest<GetBomItemBybomIdQuery>(request.url, request);
    httpRequest.Query += `bomId=${request.bomId}`;

    return await this._httpService.Post<GetBomItemBybomIdQuery, ServiceResult<PaginatedList<any>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    })

  }
}
