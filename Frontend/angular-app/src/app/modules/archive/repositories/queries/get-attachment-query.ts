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


export class GetAttachmentQuery extends IRequest<GetAttachmentQuery,PaginatedList<Attachment>>{
  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Attachment>): GetAttachmentQuery {
    throw new ApplicationError(GetAttachmentQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Attachment> {
    throw new ApplicationError(GetAttachmentQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/Attachment/GetAll";
  }
  get validationRules(): any[] {
    return [];
  }



}
@MediatorHandler(GetAttachmentQueryHandler.name)
export class GetAttachmentQueryHandler implements IRequestHandler<GetAttachmentQuery, PaginatedList<Attachment>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAttachmentQuery): Promise<PaginatedList<Attachment>> {
    let httpRequest: HttpRequest<GetAttachmentQuery> = new HttpRequest<GetAttachmentQuery>(request.url, request);


    return await this._httpService.Post<GetAttachmentQuery, ServiceResult<PaginatedList<Attachment>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
