import {PaginatedList} from "../../../../core/models/paginated-list";
import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";
import {Attachment} from "../../entities/attachment";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {ApplicationError} from "../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../core/services/http/http.service";
import {NotificationService} from "../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../core/models/service-result";


export class SearchAttachmentsQuery extends IRequest<SearchAttachmentsQuery,PaginatedList<Attachment>>{
  constructor(public pageIndex: number = 0, public pageSize: number = 0, public searchQuery: string) {
    super();
  }


  mapFrom(entity: PaginatedList<Attachment>): SearchAttachmentsQuery {
    throw new ApplicationError(SearchAttachmentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Attachment> {
    throw new ApplicationError(SearchAttachmentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/fileTransfer/Archive/SearchAttachment";
  }
  get validationRules(): any[] {
    return [];
  }



}
@MediatorHandler(SearchAttachmentsQueryHandler.name)
export class SearchAttachmentsQueryHandler implements IRequestHandler<SearchAttachmentsQuery, PaginatedList<Attachment>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: SearchAttachmentsQuery): Promise<PaginatedList<Attachment>> {
    let httpRequest: HttpRequest<SearchAttachmentsQuery> = new HttpRequest<SearchAttachmentsQuery>(request.url, request);

    httpRequest.Query += `pageIndex=${request.pageIndex}`;
    httpRequest.Query += `&pageSize=${request.pageSize}`;
    httpRequest.Query += `&searchQuery=${request.searchQuery}`;
    return await this._httpService.Get<ServiceResult<PaginatedList<Attachment>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
