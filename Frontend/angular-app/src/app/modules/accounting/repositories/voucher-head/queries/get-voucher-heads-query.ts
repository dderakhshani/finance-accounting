import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {VoucherHead} from "../../../entities/voucher-head";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";

export class GetVoucherHeadsQuery extends IRequest<GetVoucherHeadsQuery,any>{


  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }

  mapFrom(entity: PaginatedList<VoucherHead>): GetVoucherHeadsQuery {
    throw new ApplicationError(GetVoucherHeadsQuery.name,this.mapTo.name,'Not Implemented Yet')
  }

  mapTo(): PaginatedList<VoucherHead> {
    throw new ApplicationError(GetVoucherHeadsQuery.name,this.mapTo.name,'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/VouchersHead/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}
@MediatorHandler(GetVoucherHeadsQueryHandler.name)
export class GetVoucherHeadsQueryHandler implements IRequestHandler<GetVoucherHeadsQuery,PaginatedList<VoucherHead>>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }
  async Handle(request: GetVoucherHeadsQuery): Promise<any> {

    let httpRequest : HttpRequest<any> = new HttpRequest<any>(request.url,request);

    return await this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      return res.objResult;
    });
  };
}
