import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Position} from "../../../entities/position";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";

export class GetPositionQuery extends IRequest<GetPositionQuery, Position> {
  constructor(public entityId: number) {
    super();
  }




  mapFrom(entity: Position): GetPositionQuery {
    throw new ApplicationError(GetPositionQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Position {
    throw new ApplicationError(GetPositionQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/position/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPositionQueryHandler.name)
export class GetPositionQueryHandler implements IRequestHandler<GetPositionQuery, Position> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPositionQuery): Promise<Position> {
    let httpRequest: HttpRequest<GetPositionQuery> = new HttpRequest<GetPositionQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}` ;



    return await this._httpService.Get<ServiceResult<Position>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
