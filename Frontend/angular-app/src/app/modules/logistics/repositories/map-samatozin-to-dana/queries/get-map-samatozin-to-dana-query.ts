import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";

import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { Inject } from "@angular/core";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ServiceResult } from "../../../../../core/models/service-result";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MapSamatozinToDana } from "../../../entities/map-samatozin-to-dana";


export class GetMapSamatozinToDanaQuery extends IRequest<GetMapSamatozinToDanaQuery, PaginatedList<MapSamatozinToDana>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<MapSamatozinToDana>): GetMapSamatozinToDanaQuery {
    throw new ApplicationError(GetMapSamatozinToDanaQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<MapSamatozinToDana> {
    throw new ApplicationError(GetMapSamatozinToDanaQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/logistics/MapSamatozinToDana/getAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetMapSamatozinToDanaQueryHandler.name)
export class GetMapSamatozinToDanaQueryHandler implements IRequestHandler<GetMapSamatozinToDanaQuery, PaginatedList<MapSamatozinToDana>> {

  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetMapSamatozinToDanaQuery): Promise<PaginatedList<MapSamatozinToDana>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetMapSamatozinToDanaQuery> = new HttpRequest<GetMapSamatozinToDanaQuery>(request.url, request);


    return await this._httpService.Post<GetMapSamatozinToDanaQuery, ServiceResult<PaginatedList<MapSamatozinToDana>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
