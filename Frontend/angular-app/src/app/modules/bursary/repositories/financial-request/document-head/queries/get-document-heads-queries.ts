import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { DocumentHead } from "src/app/modules/bursary/entities/document-head";
import { NotificationService } from "src/app/shared/services/notification/notification.service";
import { SearchQuery } from "src/app/shared/services/search/models/search-query";

export class GetDocumentHeadsQueries extends IRequest<GetDocumentHeadsQueries, PaginatedList<DocumentHead>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<DocumentHead>): GetDocumentHeadsQueries {
    throw new ApplicationError(GetDocumentHeadsQueries.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<DocumentHead> {
    throw new ApplicationError(GetDocumentHeadsQueries.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/DocumentHead/GetDocumentHeadsByReferenceId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDocumentHeadsQueriesHandler.name)
export class GetDocumentHeadsQueriesHandler implements IRequestHandler<GetDocumentHeadsQueries, PaginatedList<DocumentHead>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDocumentHeadsQueries): Promise<PaginatedList<DocumentHead>> {
    let httpRequest: HttpRequest<GetDocumentHeadsQueries> = new HttpRequest<GetDocumentHeadsQueries>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetDocumentHeadsQueries, PaginatedList<DocumentHead>>(httpRequest).toPromise().then(response => {
      return response;
    })

  }
}
