import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {UnitPosition} from "../../../entities/unit-position";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import { Inject } from "@angular/core";

export class GetUnitPositionsQuery extends IRequest<GetUnitPositionsQuery, PaginatedList<UnitPosition>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
    this.pageIndex = pageIndex ?? 0
  }


  mapFrom(entity: PaginatedList<UnitPosition>): GetUnitPositionsQuery {
    throw new ApplicationError(GetUnitPositionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<UnitPosition> {
    throw new ApplicationError(GetUnitPositionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/unitposition/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetUnitPositionsQueryHandler.name)
export class GetUnitPositionsQueryHandler implements IRequestHandler<GetUnitPositionsQuery, PaginatedList<UnitPosition>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetUnitPositionsQuery): Promise<PaginatedList<UnitPosition>> {
    let httpRequest: HttpRequest<GetUnitPositionsQuery> = new HttpRequest<GetUnitPositionsQuery>(request.url, request);


    return await this._httpService.Post<GetUnitPositionsQuery, ServiceResult<PaginatedList<UnitPosition>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
