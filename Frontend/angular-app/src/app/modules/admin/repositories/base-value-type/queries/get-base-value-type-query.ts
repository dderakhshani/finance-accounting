import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {BaseValueType} from "../../../entities/base-value-type";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetBaseValueTypeQuery extends IRequest<GetBaseValueTypeQuery, BaseValueType> {


   constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: BaseValueType): GetBaseValueTypeQuery {
    throw new ApplicationError(GetBaseValueTypeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BaseValueType {
    throw new ApplicationError(GetBaseValueTypeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "admin/BaseValueType/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBaseValueTypeQueryHandler.name)
export class GetBaseValueTypeQueryHandler implements IRequestHandler<GetBaseValueTypeQuery, BaseValueType> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBaseValueTypeQuery): Promise<BaseValueType> {
    let httpRequest: HttpRequest<GetBaseValueTypeQuery> = new HttpRequest<GetBaseValueTypeQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Get<ServiceResult<BaseValueType>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
