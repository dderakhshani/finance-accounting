import {CountryDivision} from "../../../entities/countryDivision";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetCountryDivisionsQuery extends IRequest<GetCountryDivisionsQuery, CountryDivision[]> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CountryDivision[]): GetCountryDivisionsQuery {
    throw new ApplicationError(GetCountryDivisionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CountryDivision[] {
    throw new ApplicationError(GetCountryDivisionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/countryDivision/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCountryDivisionsQueryHandler.name)
export class GetCountryDivisionsQueryHandler implements IRequestHandler<GetCountryDivisionsQuery, CountryDivision[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCountryDivisionsQuery): Promise<CountryDivision[]> {
    let httpRequest: HttpRequest<GetCountryDivisionsQuery> = new HttpRequest<GetCountryDivisionsQuery>(request.url, request);

    return await this._httpService.Get<ServiceResult<CountryDivision[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
