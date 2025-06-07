import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {AccountReferencesGroup} from "../../../entities/account-references-group";

export class GetAccountReferencesGroupsQuery extends IRequest<GetAccountReferencesGroupsQuery, PaginatedList<AccountReferencesGroup>> {
  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<AccountReferencesGroup>): GetAccountReferencesGroupsQuery {
    throw new ApplicationError(GetAccountReferencesGroupsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountReferencesGroup> {
    throw new ApplicationError(GetAccountReferencesGroupsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountReferencesGroup/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountReferencesGroupsQueryHandler.name)
export class GetAccountReferencesGroupsQueryHandler implements IRequestHandler<GetAccountReferencesGroupsQuery, PaginatedList<AccountReferencesGroup>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAccountReferencesGroupsQuery): Promise<PaginatedList<AccountReferencesGroup>> {
    let httpRequest: HttpRequest<GetAccountReferencesGroupsQuery> = new HttpRequest<GetAccountReferencesGroupsQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetAccountReferencesGroupsQuery, ServiceResult<PaginatedList<AccountReferencesGroup>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
