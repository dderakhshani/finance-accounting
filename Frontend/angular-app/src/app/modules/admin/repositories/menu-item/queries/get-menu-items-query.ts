import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {MenuItem} from "../../../entities/menu-item";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetMenuItemsQuery extends IRequest<GetMenuItemsQuery, PaginatedList<MenuItem>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<MenuItem>): GetMenuItemsQuery {
    throw new ApplicationError(GetMenuItemsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<MenuItem> {
    throw new ApplicationError(GetMenuItemsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/MenuItem/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMenuItemsQueryHandler.name)
export class GetMenuItemsQueryHandler implements IRequestHandler<GetMenuItemsQuery, PaginatedList<MenuItem>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetMenuItemsQuery): Promise<PaginatedList<MenuItem>> {
    let httpRequest: HttpRequest<GetMenuItemsQuery> = new HttpRequest<GetMenuItemsQuery>(request.url, request);


    return await this._httpService.Post<GetMenuItemsQuery, ServiceResult<PaginatedList<MenuItem>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
