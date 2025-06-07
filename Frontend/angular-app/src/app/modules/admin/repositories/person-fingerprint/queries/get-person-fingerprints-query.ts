import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {PersonFingerprint} from "../../../entities/person-fingerprint";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetPersonFingerprintsQuery extends IRequest<GetPersonFingerprintsQuery, PaginatedList<PersonFingerprint>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<PersonFingerprint>): GetPersonFingerprintsQuery {
    throw new ApplicationError(GetPersonFingerprintsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<PersonFingerprint> {
    throw new ApplicationError(GetPersonFingerprintsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personfingerprint/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonFingerprintsQueryHandler.name)
export class GetPersonFingerprintsQueryHandler implements IRequestHandler<GetPersonFingerprintsQuery, PaginatedList<PersonFingerprint>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPersonFingerprintsQuery): Promise<PaginatedList<PersonFingerprint>> {
    let httpRequest: HttpRequest<GetPersonFingerprintsQuery> = new HttpRequest<GetPersonFingerprintsQuery>(request.url, request);

    return await this._httpService.Post<GetPersonFingerprintsQuery, ServiceResult<PaginatedList<PersonFingerprint>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
