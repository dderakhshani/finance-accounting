import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {VoucherDetail} from "../../../entities/voucher-detail";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetVoucherDetailsQuery extends IRequest<GetVoucherDetailsQuery, PaginatedList<VoucherDetail>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<VoucherDetail>): GetVoucherDetailsQuery {
    throw new ApplicationError(GetVoucherDetailsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<VoucherDetail> {
    throw new ApplicationError(GetVoucherDetailsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/VoucherDetail/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetVoucherDetailsQueryHandler.name)
export class GetVoucherDetailsQueryHandler implements IRequestHandler<GetVoucherDetailsQuery, PaginatedList<VoucherDetail>> {
  constructor(
    @Inject( HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: GetVoucherDetailsQuery): Promise<PaginatedList<VoucherDetail>> {
    let httpRequest: HttpRequest<GetVoucherDetailsQuery> = new HttpRequest<GetVoucherDetailsQuery>(request.url, request);

    return await this._httpService.Post<GetVoucherDetailsQuery, ServiceResult<PaginatedList<VoucherDetail>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
