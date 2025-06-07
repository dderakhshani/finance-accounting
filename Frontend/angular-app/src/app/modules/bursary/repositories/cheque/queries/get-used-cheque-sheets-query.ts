import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {ChequeSheet} from "../../../entities/cheque-sheet";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetUsedChequeSheetsQuery extends IRequest<GetUsedChequeSheetsQuery, PaginatedList<ChequeSheet>> {

  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }

  mapFrom(entity: PaginatedList<ChequeSheet>): GetUsedChequeSheetsQuery {
    throw new ApplicationError(GetUsedChequeSheetsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<ChequeSheet> {
    throw new ApplicationError(GetUsedChequeSheetsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/ChequeSheet/GetUsedCheques";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetUsedChequeSheetsQueryHandler.name)
export class GetUsedChequeSheetsQueryHandler implements IRequestHandler<GetUsedChequeSheetsQuery, PaginatedList<ChequeSheet>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetUsedChequeSheetsQuery): Promise<PaginatedList<ChequeSheet>> {
    let httpRequest: HttpRequest<GetUsedChequeSheetsQuery> = new HttpRequest<GetUsedChequeSheetsQuery>(request.url, request);

    return await this._httpService.Post<GetUsedChequeSheetsQuery,  PaginatedList<ChequeSheet>>(httpRequest).toPromise().then(response => {
      return response
    })

  }
}
