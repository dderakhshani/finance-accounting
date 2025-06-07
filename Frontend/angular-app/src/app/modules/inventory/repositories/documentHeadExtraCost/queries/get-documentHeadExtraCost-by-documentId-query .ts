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
import {PaginatedList} from "../../../../../core/models/paginated-list";
import { DocumentHeadExtraCost } from "../../../entities/documentHeadExtraCost";

export class GetDocumentHeadExtraCostByDocumentHeadIdQuery extends IRequest<GetDocumentHeadExtraCostByDocumentHeadIdQuery, PaginatedList<DocumentHeadExtraCost>> {

  public listIds: number[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: PaginatedList<DocumentHeadExtraCost>): GetDocumentHeadExtraCostByDocumentHeadIdQuery {
    throw new ApplicationError(GetDocumentHeadExtraCostByDocumentHeadIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<DocumentHeadExtraCost> {
    throw new ApplicationError(GetDocumentHeadExtraCostByDocumentHeadIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/DocumentHeadExtraCost/GetByDocumentHeadId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDocumentHeadExtraCostByDocumentHeadIdQueryHandler.name)
export class GetDocumentHeadExtraCostByDocumentHeadIdQueryHandler implements IRequestHandler<GetDocumentHeadExtraCostByDocumentHeadIdQuery, PaginatedList<DocumentHeadExtraCost>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDocumentHeadExtraCostByDocumentHeadIdQuery): Promise<PaginatedList<DocumentHeadExtraCost>> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetDocumentHeadExtraCostByDocumentHeadIdQuery> = new HttpRequest<GetDocumentHeadExtraCostByDocumentHeadIdQuery>(request.url, request);
   
    return await this._httpService.Post<GetDocumentHeadExtraCostByDocumentHeadIdQuery, ServiceResult<PaginatedList<DocumentHeadExtraCost>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
