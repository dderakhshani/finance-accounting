import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {BaseValue} from "../../../entities/base-value";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetBaseValueQuery extends IRequest<GetBaseValueQuery, BaseValue> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: BaseValue): GetBaseValueQuery {
    throw new ApplicationError(GetBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BaseValue {
    throw new ApplicationError(GetBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/basevalue/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBaseValueQueryHandler.name)
export class GetBaseValueQueryHandler implements IRequestHandler<GetBaseValueQuery, BaseValue> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBaseValueQuery): Promise<BaseValue> {
    let httpRequest: HttpRequest<GetBaseValueQuery> = new HttpRequest<GetBaseValueQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<BaseValue>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
