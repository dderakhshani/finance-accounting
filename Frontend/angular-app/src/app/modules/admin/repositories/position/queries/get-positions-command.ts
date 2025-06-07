import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Position} from "../../../entities/position";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetPositionsQuery extends IRequest<GetPositionsQuery, PaginatedList<Position>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Position>): GetPositionsQuery {
    throw new ApplicationError(GetPositionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Position> {
    throw new ApplicationError(GetPositionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/position/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPositionsQueryHandler.name)
export class GetPositionsQueryHandler implements IRequestHandler<GetPositionsQuery, PaginatedList<Position>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPositionsQuery): Promise<PaginatedList<Position>> {
    let httpRequest: HttpRequest<GetPositionsQuery> = new HttpRequest<GetPositionsQuery>(request.url, request);


    return await this._httpService.Post<GetPositionsQuery, ServiceResult<PaginatedList<Position>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
