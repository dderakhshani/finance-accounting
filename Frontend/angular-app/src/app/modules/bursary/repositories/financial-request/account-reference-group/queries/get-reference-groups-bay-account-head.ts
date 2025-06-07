import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { AccountReferencesGroup } from "src/app/modules/bursary/entities/account-reference-group";
import { NotificationService } from "src/app/shared/services/notification/notification.service";

export class GetReferenceGroupsByAccountHeadIdQuery extends IRequest<GetReferenceGroupsByAccountHeadIdQuery, PaginatedList<AccountReferencesGroup>> {



  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: PaginatedList<AccountReferencesGroup>): GetReferenceGroupsByAccountHeadIdQuery {
    throw new ApplicationError(GetReferenceGroupsByAccountHeadIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountReferencesGroup> {
    throw new ApplicationError(GetReferenceGroupsByAccountHeadIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/AccountReferenceGroup/GetReferencesGroupBy";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetReferenceGroupsByAccountHeadIdQueryHandler.name)
export class GetReferenceGroupsByAccountHeadIdQueryHandler implements IRequestHandler<GetReferenceGroupsByAccountHeadIdQuery, PaginatedList<AccountReferencesGroup>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetReferenceGroupsByAccountHeadIdQuery): Promise<PaginatedList<AccountReferencesGroup>> {
    let httpRequest: HttpRequest<GetReferenceGroupsByAccountHeadIdQuery> = new HttpRequest<GetReferenceGroupsByAccountHeadIdQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<PaginatedList<AccountReferencesGroup>>(httpRequest).toPromise().then(response => {
      return response
    })
  }
}
