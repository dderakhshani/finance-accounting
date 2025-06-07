import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { AccountReference } from "src/app/modules/bursary/entities/account-reference";

import { DocumentHead } from "src/app/modules/bursary/entities/document-head";
import { NotificationService } from "src/app/shared/services/notification/notification.service";
import { SearchQuery } from "src/app/shared/services/search/models/search-query";

export class GetBursaryAccountReferencesQuery extends IRequest<GetBursaryAccountReferencesQuery, PaginatedList<AccountReference>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<AccountReference>): GetBursaryAccountReferencesQuery {
    throw new ApplicationError(GetBursaryAccountReferencesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountReference> {
    throw new ApplicationError(GetBursaryAccountReferencesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/AccountReference/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBursaryAccountReferencesQueryHandler.name)
export class GetBursaryAccountReferencesQueryHandler implements IRequestHandler<GetBursaryAccountReferencesQuery, PaginatedList<AccountReference>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBursaryAccountReferencesQuery): Promise<PaginatedList<AccountReference>> {
    let httpRequest: HttpRequest<GetBursaryAccountReferencesQuery> = new HttpRequest<GetBursaryAccountReferencesQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetBursaryAccountReferencesQuery, PaginatedList<AccountReference>>(httpRequest).toPromise().then(response => {
      return response;
    })

  }
}
