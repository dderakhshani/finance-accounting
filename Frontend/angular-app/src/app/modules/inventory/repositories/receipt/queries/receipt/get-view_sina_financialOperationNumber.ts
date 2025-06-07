import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";

import { PaginatedList } from "../../../../../../core/models/paginated-list";
import { View_Sina_FinancialOperationNumber } from "../../../../entities/receipt";


export class GeView_Sina_FinancialOperationNumberQuery extends IRequest<GeView_Sina_FinancialOperationNumberQuery, PaginatedList<View_Sina_FinancialOperationNumber>> {


  constructor(

    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = ''
  ) {
    super();
  }

  mapFrom(entity: PaginatedList<View_Sina_FinancialOperationNumber>): GeView_Sina_FinancialOperationNumberQuery {
    throw new ApplicationError(GeView_Sina_FinancialOperationNumberQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<View_Sina_FinancialOperationNumber> {
    throw new ApplicationError(GeView_Sina_FinancialOperationNumberQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GeView_Sina_FinancialOperationNumber";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GeView_Sina_FinancialOperationNumberQueryHandler.name)
export class GeView_Sina_FinancialOperationNumberQueryHandler implements IRequestHandler<GeView_Sina_FinancialOperationNumberQuery, PaginatedList<View_Sina_FinancialOperationNumber>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GeView_Sina_FinancialOperationNumberQuery): Promise<PaginatedList<View_Sina_FinancialOperationNumber>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GeView_Sina_FinancialOperationNumberQuery> = new HttpRequest<GeView_Sina_FinancialOperationNumberQuery>(request.url, request);

   
    return await this._httpService.Post<GeView_Sina_FinancialOperationNumberQuery, ServiceResult<PaginatedList<View_Sina_FinancialOperationNumber>>>(httpRequest).toPromise().then(response => {
    
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
