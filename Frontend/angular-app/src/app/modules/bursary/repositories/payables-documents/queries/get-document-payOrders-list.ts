import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";

import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {documentPayOrders} from "../../../entities/documentPayOrders";

export class GetDocumentPayOrdersListQuery extends IRequest<GetDocumentPayOrdersListQuery,documentPayOrders[]>{
  constructor(pageIndex:number = 0, pageSize:number = 0, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();
    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }


  mapFrom(entity:documentPayOrders[]): GetDocumentPayOrdersListQuery {
    throw new ApplicationError(GetDocumentPayOrdersListQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo():documentPayOrders[] {
    throw new ApplicationError(GetDocumentPayOrdersListQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url():string {
    return "/bursary/Payables_Documents/GetDocumentPayOrdersList";
  }

  get validationRules():ValidationRule[] {
    return [];
  }

}
@MediatorHandler(GetDocumentPayOrdersListQueryHandler.name)
export class GetDocumentPayOrdersListQueryHandler implements IRequestHandler<GetDocumentPayOrdersListQuery, documentPayOrders[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDocumentPayOrdersListQuery): Promise<documentPayOrders[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetDocumentPayOrdersListQuery> = new HttpRequest<GetDocumentPayOrdersListQuery>(request.url, request);

    return await this._httpService.Post<GetDocumentPayOrdersListQuery,  documentPayOrders[]>(httpRequest).toPromise().then((response:any) => {
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
