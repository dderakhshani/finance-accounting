import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {Language} from "../../../entities/language";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetLanguageQuery extends IRequest<GetLanguageQuery, Language> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: Language): GetLanguageQuery {
    throw new ApplicationError(GetLanguageQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Language {
    throw new ApplicationError(GetLanguageQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/language/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetLanguageQueryHandler.name)
export class GetLanguageQueryHandler implements IRequestHandler<GetLanguageQuery, Language> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetLanguageQuery): Promise<Language> {
    let httpRequest: HttpRequest<GetLanguageQuery> = new HttpRequest<GetLanguageQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Language>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
