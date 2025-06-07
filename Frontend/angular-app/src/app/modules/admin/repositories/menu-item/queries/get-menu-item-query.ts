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

export class GetMenuItemQuery extends IRequest<GetMenuItemQuery, MenuItem> {


   constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: MenuItem): GetMenuItemQuery {
    throw new ApplicationError(GetMenuItemQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MenuItem {
    throw new ApplicationError(GetMenuItemQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/menuItem/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMenuItemQueryHandler.name)
export class GetMenuItemQueryHandler implements IRequestHandler<GetMenuItemQuery, MenuItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetMenuItemQuery): Promise<MenuItem> {
    let httpRequest: HttpRequest<GetMenuItemQuery> = new HttpRequest<GetMenuItemQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Get<ServiceResult<MenuItem>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
