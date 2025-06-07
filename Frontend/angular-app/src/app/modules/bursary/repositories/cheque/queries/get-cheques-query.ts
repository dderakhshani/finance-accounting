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
import {Cheque} from "../../../entities/cheque";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetChequesQuery extends IRequest<GetChequesQuery, PaginatedList<Cheque>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Cheque>): GetChequesQuery {
    throw new ApplicationError(GetChequesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Cheque> {
    throw new ApplicationError(GetChequesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/cheque/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetChequesQueryHandler.name)
export class GetChequesQueryHandler implements IRequestHandler<GetChequesQuery, PaginatedList<Cheque>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetChequesQuery): Promise<PaginatedList<Cheque>> {
    let httpRequest: HttpRequest<GetChequesQuery> = new HttpRequest<GetChequesQuery>(request.url, request);

    return await this._httpService.Post<GetChequesQuery, ServiceResult<PaginatedList<Cheque>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
