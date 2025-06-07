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

export class GetDocumentHeadExtraCostsQuery extends IRequest<GetDocumentHeadExtraCostsQuery, PaginatedList<DocumentHeadExtraCost>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<DocumentHeadExtraCost>): GetDocumentHeadExtraCostsQuery {
    throw new ApplicationError(GetDocumentHeadExtraCostsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<DocumentHeadExtraCost> {
    throw new ApplicationError(GetDocumentHeadExtraCostsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/DocumentHeadExtraCost/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDocumentHeadExtraCostsQueryHandler.name)
export class GetDocumentHeadExtraCostsQueryHandler implements IRequestHandler<GetDocumentHeadExtraCostsQuery, PaginatedList<DocumentHeadExtraCost>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDocumentHeadExtraCostsQuery): Promise<PaginatedList<DocumentHeadExtraCost>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetDocumentHeadExtraCostsQuery> = new HttpRequest<GetDocumentHeadExtraCostsQuery>(request.url, request);


    return await this._httpService.Post<GetDocumentHeadExtraCostsQuery, ServiceResult<PaginatedList<DocumentHeadExtraCost>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
