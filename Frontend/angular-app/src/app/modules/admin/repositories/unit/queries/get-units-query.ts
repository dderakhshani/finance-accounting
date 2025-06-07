import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Unit} from "../../../entities/unit";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetUnitsQuery extends IRequest<GetUnitsQuery, PaginatedList<Unit>> {
  constructor(pageIndex?: number, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
    this.pageIndex = pageIndex ?? 0
  }


  mapFrom(entity: PaginatedList<Unit>): GetUnitsQuery {
    throw new ApplicationError(GetUnitsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Unit> {
    throw new ApplicationError(GetUnitsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/unit/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetUnitsQueryHandler.name)
export class GetUnitsQueryHandler implements IRequestHandler<GetUnitsQuery, PaginatedList<Unit>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetUnitsQuery): Promise<PaginatedList<Unit>> {
    let httpRequest: HttpRequest<GetUnitsQuery> = new HttpRequest<GetUnitsQuery>(request.url, request);

    return await this._httpService.Post<GetUnitsQuery, ServiceResult<PaginatedList<Unit>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
