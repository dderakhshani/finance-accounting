import {Inject} from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { AccountReference } from "src/app/modules/bursary/entities/account-reference";
import { NotificationService } from "src/app/shared/services/notification/notification.service";


export class GetAccountReferenceByGroupIdQuery extends IRequest<GetAccountReferenceByGroupIdQuery, PaginatedList<AccountReference>> {

  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: PaginatedList<AccountReference>): GetAccountReferenceByGroupIdQuery {
    throw new ApplicationError(GetAccountReferenceByGroupIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountReference> {
    throw new ApplicationError(GetAccountReferenceByGroupIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/AccountReference/ReferenceAccountsByGroupId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountReferenceByGroupIdQueryHandler.name)
export class GetAccountReferenceByGroupIdQueryHandler implements IRequestHandler<GetAccountReferenceByGroupIdQuery, PaginatedList<AccountReference>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAccountReferenceByGroupIdQuery): Promise<PaginatedList<AccountReference>> {
    let httpRequest: HttpRequest<GetAccountReferenceByGroupIdQuery> = new HttpRequest<GetAccountReferenceByGroupIdQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<PaginatedList<AccountReference>>(httpRequest).toPromise().then(response => {
      return response
    })
  }
}
