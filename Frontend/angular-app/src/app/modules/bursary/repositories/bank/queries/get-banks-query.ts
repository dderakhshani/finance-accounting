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
import { Bank } from "../../../entities/bank";

export class GetBanksQuery extends IRequest<GetBanksQuery, PaginatedList<Bank>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 100, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Bank>): GetBanksQuery {
    throw new ApplicationError(GetBanksQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Bank> {
    throw new ApplicationError(GetBanksQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/Definitions/banks/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBanksQueryHandler.name)
export class GetBanksQueryHandler implements IRequestHandler<GetBanksQuery, PaginatedList<Bank>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBanksQuery): Promise<PaginatedList<Bank>> {
    let httpRequest: HttpRequest<GetBanksQuery> = new HttpRequest<GetBanksQuery>(request.url, request);

    return await this._httpService.Post<GetBanksQuery,  PaginatedList<Bank>>(httpRequest).toPromise().then(response => {
      return response
    })

  }
}
