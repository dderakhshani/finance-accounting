import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {Year} from "../../../entities/year";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";

export class GetYearQuery extends IRequest<GetYearQuery, Year> {
  constructor(public entityId: number) {
    super();
  }




  mapFrom(entity: Year): GetYearQuery {
    throw new ApplicationError(GetYearQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Year {
    throw new ApplicationError(GetYearQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/year/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetYearQueryHandler.name)
export class GetYearQueryHandler implements IRequestHandler<GetYearQuery, Year> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetYearQuery): Promise<Year> {
    let httpRequest: HttpRequest<GetYearQuery> = new HttpRequest<GetYearQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}` ;


    return await this._httpService.Get<ServiceResult<Year>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
