import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { AccountReferencesGroup } from "src/app/modules/bursary/entities/account-reference-group";
import { NotificationService } from "src/app/shared/services/notification/notification.service";
import { SearchQuery } from "src/app/shared/services/search/models/search-query";

export class GetBursaryAccountReferenceGroupsQuery extends IRequest<GetBursaryAccountReferenceGroupsQuery, PaginatedList<AccountReferencesGroup>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<AccountReferencesGroup>): GetBursaryAccountReferenceGroupsQuery {
    throw new ApplicationError(GetBursaryAccountReferenceGroupsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountReferencesGroup> {
    throw new ApplicationError(GetBursaryAccountReferenceGroupsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/AccountReferenceGroup/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBursaryAccountReferenceGroupsQueryHandler.name)
export class GetBursaryAccountReferenceGroupsQueryHandler implements IRequestHandler<GetBursaryAccountReferenceGroupsQuery, PaginatedList<AccountReferencesGroup>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBursaryAccountReferenceGroupsQuery): Promise<PaginatedList<AccountReferencesGroup>> {
    let httpRequest: HttpRequest<GetBursaryAccountReferenceGroupsQuery> = new HttpRequest<GetBursaryAccountReferenceGroupsQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetBursaryAccountReferenceGroupsQuery, PaginatedList<AccountReferencesGroup>>(httpRequest).toPromise().then(response => {
      return response;
    })

  }
}
