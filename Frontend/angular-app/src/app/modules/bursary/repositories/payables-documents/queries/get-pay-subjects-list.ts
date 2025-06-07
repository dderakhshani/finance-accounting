import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {BaseValue} from "../../../../admin/entities/base-value";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";

import {SearchQuery} from "../../../../../shared/services/search/models/search-query";

export class GetPaySubjectsListQuery extends IRequest<GetPaySubjectsListQuery,BaseValue[]>{
  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }


  mapFrom(entity:BaseValue[]): GetPaySubjectsListQuery {
    throw new ApplicationError(GetPaySubjectsListQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo():BaseValue[] {
    throw new ApplicationError(GetPaySubjectsListQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url():string {
    return "/bursary/Payables_Documents/GetPaySubjectsList";
  }

  get validationRules():ValidationRule[] {
    return [];
  }

}
@MediatorHandler(GetPaySubjectsListQueryHandler.name)
export class GetPaySubjectsListQueryHandler implements IRequestHandler<GetPaySubjectsListQuery, BaseValue[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPaySubjectsListQuery): Promise<BaseValue[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetPaySubjectsListQuery> = new HttpRequest<GetPaySubjectsListQuery>(request.url, request);

    return await this._httpService.Post<GetPaySubjectsListQuery,  BaseValue[]>(httpRequest).toPromise().then((response:any) => {
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
