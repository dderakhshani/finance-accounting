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

export class GetChqueSheetsQuery extends IRequest<GetChqueSheetsQuery, PaginatedList<ChequeSheet>> {

  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }



  mapFrom(entity: PaginatedList<ChequeSheet>): GetChqueSheetsQuery {
    throw new ApplicationError(GetChqueSheetsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<ChequeSheet> {
    throw new ApplicationError(GetChqueSheetsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/ChequeSheet/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetChqueSheetsQueryHandler.name)
export class GetChqueSheetsQueryHandler implements IRequestHandler<GetChqueSheetsQuery, PaginatedList<ChequeSheet>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetChqueSheetsQuery): Promise<PaginatedList<ChequeSheet>> {
    let httpRequest: HttpRequest<GetChqueSheetsQuery> = new HttpRequest<GetChqueSheetsQuery>(request.url, request);

    return await this._httpService.Post<GetChqueSheetsQuery,  PaginatedList<ChequeSheet>>(httpRequest).toPromise().then(response => {
      return response
    })

  }
}
