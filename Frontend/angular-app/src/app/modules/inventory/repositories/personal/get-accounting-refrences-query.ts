import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../core/models/paginated-list";
import { AccountReference } from "../../../accounting/entities/account-reference";

export class GetAccountReferenceInventoryQuery extends IRequest<GetAccountReferenceInventoryQuery, PaginatedList<AccountReference>> {

  constructor(public accountHeadId: number | undefined =undefined, public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<AccountReference>): GetAccountReferenceInventoryQuery {
    throw new ApplicationError(GetAccountReferenceInventoryQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountReference> {
    throw new ApplicationError(GetAccountReferenceInventoryQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/AccountReferences/GetAccountReferences";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountReferenceInventoryQueryHandler.name)
export class GetAccountReferenceInventoryQueryHandler implements IRequestHandler<GetAccountReferenceInventoryQuery, PaginatedList<AccountReference>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAccountReferenceInventoryQuery): Promise<PaginatedList<AccountReference>> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetAccountReferenceInventoryQuery> = new HttpRequest<GetAccountReferenceInventoryQuery>(request.url, request);
    if (request.accountHeadId != undefined) {
      httpRequest.Query += `accountHeadId=${request.accountHeadId}`;
    }
    
    return await this._httpService.Post<GetAccountReferenceInventoryQuery, ServiceResult<PaginatedList<AccountReference>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
