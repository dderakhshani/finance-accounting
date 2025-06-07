import {CountryDivision} from "../../../entities/countryDivision";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCountryDivisionQuery extends IRequest<GetCountryDivisionQuery, CountryDivision> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: CountryDivision): GetCountryDivisionQuery {
    throw new ApplicationError(GetCountryDivisionQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CountryDivision {
    throw new ApplicationError(GetCountryDivisionQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/countryDivision/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCountryDivisionQueryHandler.name)
export class GetCountryDivisionQueryHandler implements IRequestHandler<GetCountryDivisionQuery, CountryDivision> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCountryDivisionQuery): Promise<CountryDivision> {
    let httpRequest: HttpRequest<GetCountryDivisionQuery> = new HttpRequest<GetCountryDivisionQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<CountryDivision>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
