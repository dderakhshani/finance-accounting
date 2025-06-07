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
import { Assets } from "../../../entities/Assets";

export class GetAssetsQuery extends IRequest<GetAssetsQuery, Assets> {


   constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Assets): GetAssetsQuery {
    throw new ApplicationError(GetAssetsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Assets {
    throw new ApplicationError(GetAssetsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "inventory/Assets/Get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAssetsQueryHandler.name)
export class GetAssetsQueryHandler implements IRequestHandler<GetAssetsQuery, Assets> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAssetsQuery): Promise<Assets> {
    let httpRequest: HttpRequest<GetAssetsQuery> = new HttpRequest<GetAssetsQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Get<ServiceResult<Assets>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
