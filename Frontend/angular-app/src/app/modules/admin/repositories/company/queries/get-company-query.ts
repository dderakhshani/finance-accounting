import {Inject} from "@angular/core";
import {Company} from "../../../entities/company";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCompanyQuery extends IRequest<GetCompanyQuery, Company> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Company): GetCompanyQuery {
    throw new ApplicationError(GetCompanyQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Company {
    throw new ApplicationError(GetCompanyQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/companyInformation/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCompanyQueryHandler.name)
export class GetCompanyQueryHandler implements IRequestHandler<GetCompanyQuery, Company> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCompanyQuery): Promise<Company> {
    let httpRequest: HttpRequest<GetCompanyQuery> = new HttpRequest<GetCompanyQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Company>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
