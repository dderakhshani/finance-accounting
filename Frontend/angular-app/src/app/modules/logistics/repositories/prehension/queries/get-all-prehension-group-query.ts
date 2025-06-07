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

export class GetPrehensionGroupByCodeQuery extends IRequest<GetPrehensionGroupByCodeQuery, PaginatedList<string>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<string>): GetPrehensionGroupByCodeQuery {
    throw new ApplicationError(GetPrehensionGroupByCodeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<string> {
    throw new ApplicationError(GetPrehensionGroupByCodeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/logistics/Prehension/GetGroupByCode";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetPrehensionGroupByCodeQueryHandler.name)
export class GetPrehensionGroupByCodeQueryHandler implements IRequestHandler<GetPrehensionGroupByCodeQuery, PaginatedList<string>> {

  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPrehensionGroupByCodeQuery): Promise<PaginatedList<string>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetPrehensionGroupByCodeQuery> = new HttpRequest<GetPrehensionGroupByCodeQuery>(request.url, request);


    return await this._httpService.Post<GetPrehensionGroupByCodeQuery, ServiceResult<PaginatedList<string>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
