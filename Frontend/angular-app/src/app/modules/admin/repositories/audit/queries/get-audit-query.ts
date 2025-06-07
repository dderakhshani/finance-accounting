import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Audit} from "../../../entities/audit";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetAuditQuery extends IRequest<GetAuditQuery, PaginatedList<Audit>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Audit>): GetAuditQuery {
    throw new ApplicationError(GetAuditQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Audit> {
    throw new ApplicationError(GetAuditQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/Audit/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAuditQueryHandler.name)
export class GetAuditQueryHandler implements IRequestHandler<GetAuditQuery, PaginatedList<Audit>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAuditQuery): Promise<PaginatedList<Audit>> {

    let httpRequest: HttpRequest<GetAuditQuery> = new HttpRequest<GetAuditQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    // @ts-ignore
    return await this._httpService.Post<GetAuditQuery, ServiceResult<PaginatedList<Audit>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
