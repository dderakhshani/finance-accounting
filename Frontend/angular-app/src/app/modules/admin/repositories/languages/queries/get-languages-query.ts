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
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetLanguagesQuery extends IRequest<GetLanguagesQuery, PaginatedList<Language>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Language>): GetLanguagesQuery {
    throw new ApplicationError(GetLanguagesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Language> {
    throw new ApplicationError(GetLanguagesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/language/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetLanguagesQueryHandler.name)
export class GetLanguagesQueryHandler implements IRequestHandler<GetLanguagesQuery, PaginatedList<Language>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetLanguagesQuery): Promise<PaginatedList<Language>> {
    let httpRequest: HttpRequest<GetLanguagesQuery> = new HttpRequest<GetLanguagesQuery>(request.url, request);

    return await this._httpService.Post<GetLanguagesQuery, ServiceResult<PaginatedList<Language>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
