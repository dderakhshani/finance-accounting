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


export class GetAccountReferencesPersonQuery extends IRequest<GetAccountReferencesPersonQuery, PaginatedList<AccountReference>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<AccountReference>): GetAccountReferencesPersonQuery {
    throw new ApplicationError(GetAccountReferencesPersonQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountReference> {
    throw new ApplicationError(GetAccountReferencesPersonQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/AccountReferences/GetAccountReferencesPerson";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetAccountReferencesPersonQueryHandler.name)
export class GetAccountReferencesPersonQueryHandler implements IRequestHandler<GetAccountReferencesPersonQuery, PaginatedList<AccountReference>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAccountReferencesPersonQuery): Promise<PaginatedList<AccountReference>> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetAccountReferencesPersonQuery> = new HttpRequest<GetAccountReferencesPersonQuery>(request.url, request);


    return await this._httpService.Post<GetAccountReferencesPersonQuery, ServiceResult<PaginatedList<AccountReference>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
