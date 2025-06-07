import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";

import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { Inject } from "@angular/core";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ServiceResult } from "../../../../../core/models/service-result";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { Prehension } from "../../../entities/prehension";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { ApplicationError } from "../../../../../core/exceptions/application-error";

export class GetPrehensionQuery extends IRequest<GetPrehensionQuery, PaginatedList<Prehension>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Prehension>): GetPrehensionQuery {
    throw new ApplicationError(GetPrehensionQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Prehension> {
    throw new ApplicationError(GetPrehensionQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/logistics/Prehension/getAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetPrehensionQueryHandler.name)
export class GetPrehensionQueryHandler implements IRequestHandler<GetPrehensionQuery, PaginatedList<Prehension>> {

  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPrehensionQuery): Promise<PaginatedList<Prehension>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetPrehensionQuery> = new HttpRequest<GetPrehensionQuery>(request.url, request);


    return await this._httpService.Post<GetPrehensionQuery, ServiceResult<PaginatedList<Prehension>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
