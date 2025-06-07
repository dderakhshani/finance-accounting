import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {Year} from "../../../entities/year";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetYearsQuery extends IRequest<GetYearsQuery, PaginatedList<Year>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Year>): GetYearsQuery {
    throw new ApplicationError(GetYearsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Year> {
    throw new ApplicationError(GetYearsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/year/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetYearsQueryHandler.name)
export class GetYearsQueryHandler implements IRequestHandler<GetYearsQuery, PaginatedList<Year>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetYearsQuery): Promise<PaginatedList<Year>> {
    let httpRequest: HttpRequest<GetYearsQuery> = new HttpRequest<GetYearsQuery>(request.url, request);

    return await this._httpService.Post<GetYearsQuery, ServiceResult<PaginatedList<Year>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
