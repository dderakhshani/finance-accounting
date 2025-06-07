import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {BaseValue} from "../../../entities/base-value";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetBaseValuesByUniqueNameQuery extends IRequest<GetBaseValuesByUniqueNameQuery, BaseValue[]> {

  constructor(public uniqueName: string) {
    super();
  }


  mapFrom(entity:BaseValue[]): GetBaseValuesByUniqueNameQuery {
    throw new ApplicationError(GetBaseValuesByUniqueNameQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo():BaseValue[] {
    throw new ApplicationError(GetBaseValuesByUniqueNameQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url():string {
    return "/admin/BaseValue/GetAllByCategoryUniqueName";
  }

  get validationRules():ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBaseValuesByUniqueNameQueryHandler.name)
export class GetBaseValuesByUniqueNameQueryHandler implements IRequestHandler<GetBaseValuesByUniqueNameQuery, BaseValue[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBaseValuesByUniqueNameQuery): Promise<BaseValue[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetBaseValuesByUniqueNameQuery> = new HttpRequest<GetBaseValuesByUniqueNameQuery>(request.url, request);
    httpRequest.Query  += `BaseValueTypeUniqueName=${request.uniqueName}`;


    return await this._httpService.Get<ServiceResult<BaseValue[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
