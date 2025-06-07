import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Unit} from "../../../entities/unit";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";

export class GetUnitQuery extends IRequest<GetUnitQuery, Unit> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: Unit): GetUnitQuery {
    throw new ApplicationError(GetUnitQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Unit {
    throw new ApplicationError(GetUnitQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/unit/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetUnitQueryHandler.name)
export class GetUnitQueryHandler implements IRequestHandler<GetUnitQuery, Unit> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetUnitQuery): Promise<Unit> {
    let httpRequest: HttpRequest<GetUnitQuery> = new HttpRequest<GetUnitQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}` ;



    return await this._httpService.Get<ServiceResult<Unit>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
