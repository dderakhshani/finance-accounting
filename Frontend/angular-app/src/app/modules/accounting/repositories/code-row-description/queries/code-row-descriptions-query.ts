import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {CodeRowDescription} from "../../../entities/code-row-description";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCodeRowDescriptionsQuery extends IRequest<GetCodeRowDescriptionsQuery, PaginatedList<CodeRowDescription>> {
  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }




  mapFrom(entity: PaginatedList<CodeRowDescription>): GetCodeRowDescriptionsQuery {
    throw new ApplicationError(GetCodeRowDescriptionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<CodeRowDescription> {
    throw new ApplicationError(GetCodeRowDescriptionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeRowDescription/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCodeRowDescriptionsQueryHandler.name)
export class GetCodeRowDescriptionsQueryHandler implements IRequestHandler<GetCodeRowDescriptionsQuery, PaginatedList<CodeRowDescription>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCodeRowDescriptionsQuery): Promise<PaginatedList<CodeRowDescription>> {
    let httpRequest: HttpRequest<GetCodeRowDescriptionsQuery> = new HttpRequest<GetCodeRowDescriptionsQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetCodeRowDescriptionsQuery, ServiceResult<PaginatedList<CodeRowDescription>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

    return await this._httpService.Get<ServiceResult<PaginatedList<CodeRowDescription>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
