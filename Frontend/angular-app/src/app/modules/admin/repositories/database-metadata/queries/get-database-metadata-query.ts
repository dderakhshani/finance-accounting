import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {DatabaseMetadata} from "../../../entities/DatabaseMetadata";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetDatabaseMetadataQuery extends IRequest<GetDatabaseMetadataQuery, PaginatedList<DatabaseMetadata>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<DatabaseMetadata>): GetDatabaseMetadataQuery {
    throw new ApplicationError(GetDatabaseMetadataQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<DatabaseMetadata> {
    throw new ApplicationError(GetDatabaseMetadataQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Admin/DatabaseMetadata/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDatabaseMetadataQueryHandler.name)
export class GetDatabaseMetadataQueryHandler implements IRequestHandler<GetDatabaseMetadataQuery, PaginatedList<DatabaseMetadata>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDatabaseMetadataQuery): Promise<PaginatedList<DatabaseMetadata>> {
    let httpRequest: HttpRequest<GetDatabaseMetadataQuery> = new HttpRequest<GetDatabaseMetadataQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetDatabaseMetadataQuery, ServiceResult<PaginatedList<DatabaseMetadata>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
