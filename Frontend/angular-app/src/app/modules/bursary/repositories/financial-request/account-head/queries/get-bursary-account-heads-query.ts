import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { AccountHead } from "src/app/modules/bursary/entities/account-head";
import { NotificationService } from "src/app/shared/services/notification/notification.service";
import { SearchQuery } from "src/app/shared/services/search/models/search-query";

export class GetBursaryAccountHeadsQuery extends IRequest<GetBursaryAccountHeadsQuery, PaginatedList<AccountHead>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<AccountHead>): GetBursaryAccountHeadsQuery {
    throw new ApplicationError(GetBursaryAccountHeadsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountHead> {
    throw new ApplicationError(GetBursaryAccountHeadsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/AccountHead/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBursaryAccountHeadsQueryHandler.name)
export class GetBursaryAccountHeadsQueryHandler implements IRequestHandler<GetBursaryAccountHeadsQuery, PaginatedList<AccountHead>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBursaryAccountHeadsQuery): Promise<PaginatedList<AccountHead>> {
    let httpRequest: HttpRequest<GetBursaryAccountHeadsQuery> = new HttpRequest<GetBursaryAccountHeadsQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetBursaryAccountHeadsQuery, PaginatedList<AccountHead>>(httpRequest).toPromise().then(response => {
      return response;
    })

  }
}
