import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {BaseValue} from "../../../../admin/entities/base-value";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";

import {SearchQuery} from "../../../../../shared/services/search/models/search-query";

export class GetChequeTypesQuery extends IRequest<GetChequeTypesQuery,BaseValue[]>{
  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }


  mapFrom(entity:BaseValue[]): GetChequeTypesQuery {
    throw new ApplicationError(GetChequeTypesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo():BaseValue[] {
    throw new ApplicationError(GetChequeTypesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url():string {
    return "/bursary/Chequetypes/GetAll";
  }

  get validationRules():ValidationRule[] {
    return [];
  }

}
@MediatorHandler(GetChequeTypesQueryHandler.name)
export class GetChequeTypesQueryHandler implements IRequestHandler<GetChequeTypesQuery, BaseValue[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetChequeTypesQuery): Promise<BaseValue[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetChequeTypesQuery> = new HttpRequest<GetChequeTypesQuery>(request.url, request);

    return await this._httpService.Post<GetChequeTypesQuery,  BaseValue[]>(httpRequest).toPromise().then((response:any) => {
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
