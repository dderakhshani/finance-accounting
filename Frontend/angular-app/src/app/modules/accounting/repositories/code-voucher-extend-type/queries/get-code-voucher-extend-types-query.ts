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
import {CodeVoucherExtendType} from "../../../entities/code-voucher-extend-type";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetCodeVoucherExtendTypesQuery extends IRequest<GetCodeVoucherExtendTypesQuery, PaginatedList<CodeVoucherExtendType>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<CodeVoucherExtendType>): GetCodeVoucherExtendTypesQuery {
    throw new ApplicationError(GetCodeVoucherExtendTypesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<CodeVoucherExtendType> {
    throw new ApplicationError(GetCodeVoucherExtendTypesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherExtendType/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCodeVoucherExtendTypesQueryHandler.name)
export class GetCodeVoucherExtendTypesQueryHandler implements IRequestHandler<GetCodeVoucherExtendTypesQuery, PaginatedList<CodeVoucherExtendType>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCodeVoucherExtendTypesQuery): Promise<PaginatedList<CodeVoucherExtendType>> {
    let httpRequest: HttpRequest<GetCodeVoucherExtendTypesQuery> = new HttpRequest<GetCodeVoucherExtendTypesQuery>(request.url, request);

    return await this._httpService.Post<GetCodeVoucherExtendTypesQuery, ServiceResult<PaginatedList<CodeVoucherExtendType>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
