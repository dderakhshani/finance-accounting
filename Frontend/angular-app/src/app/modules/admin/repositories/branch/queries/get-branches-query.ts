import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Branch} from "../../../entities/branch";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetBranchesQuery extends IRequest<GetBranchesQuery, PaginatedList<Branch>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Branch>): GetBranchesQuery {
    throw new ApplicationError(GetBranchesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Branch> {
    throw new ApplicationError(GetBranchesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/branch/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBranchesQueryHandler.name)
export class GetBranchesQueryHandler implements IRequestHandler<GetBranchesQuery, PaginatedList<Branch>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBranchesQuery): Promise<PaginatedList<Branch>> {
    let httpRequest: HttpRequest<GetBranchesQuery> = new HttpRequest<GetBranchesQuery>(request.url, request);

    return await this._httpService.Post<GetBranchesQuery, ServiceResult<PaginatedList<Branch>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
