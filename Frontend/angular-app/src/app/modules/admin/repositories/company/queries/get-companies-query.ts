import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {Company} from "../../../entities/company";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCompaniesQuery extends IRequest<GetCompaniesQuery, PaginatedList<Company>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Company>): GetCompaniesQuery {
    throw new ApplicationError(GetCompaniesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Company> {
    throw new ApplicationError(GetCompaniesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/companyInformation/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCompaniesQueryHandler.name)
export class GetCompaniesQueryHandler implements IRequestHandler<GetCompaniesQuery, PaginatedList<Company>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCompaniesQuery): Promise<PaginatedList<Company>> {
    let httpRequest: HttpRequest<GetCompaniesQuery> = new HttpRequest<GetCompaniesQuery>(request.url, request);

    return await this._httpService.Post<GetCompaniesQuery, ServiceResult<PaginatedList<Company>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
