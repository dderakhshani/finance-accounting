import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";

import {ValidationRule} from "../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../core/models/service-result";
import {PaginatedList} from "../../../../core/models/paginated-list";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import { AccountHead } from "../../../accounting/entities/account-head";
import { NotificationService } from "../../../../shared/services/notification/notification.service";

export class GetInventoryAccountHeadsQuery extends IRequest<GetInventoryAccountHeadsQuery, AccountHead[]> {

  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }
  mapFrom(entity: AccountHead[]): GetInventoryAccountHeadsQuery {
    return new GetInventoryAccountHeadsQuery();
  }

  mapTo(): AccountHead[] {
    return [];
  }

  get url(): string {
    return "/inventory/AccountReferences/GetAccountHead";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetInventoryAccountHeadsQueryHandler.name)
export class GetInventoryAccountHeadsQueryHandler implements IRequestHandler<GetInventoryAccountHeadsQuery, AccountHead[]>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
   
  ) {
  }

  async Handle(request: GetInventoryAccountHeadsQuery): Promise<AccountHead[]> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any, ServiceResult<PaginatedList<AccountHead>>>(httpRequest).toPromise().then(res => {
     
      return res.objResult.data
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })
  }
}
