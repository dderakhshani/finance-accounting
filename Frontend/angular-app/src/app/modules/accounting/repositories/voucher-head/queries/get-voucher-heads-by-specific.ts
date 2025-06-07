import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {VoucherHead} from "../../../entities/voucher-head";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {GetVoucherHeadsQuery} from "./get-voucher-heads-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";


export class GetVoucherHeadsBySpecificQuery extends IRequest<GetVoucherHeadsBySpecificQuery,any> {
  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries ?? [];
    this.orderByProperty = orderByProperty ?? '';
  }

  public fromDateTime:Date | undefined = undefined;
  public toDateTime:Date | undefined = undefined;
  public fromNo:number | undefined = undefined;
  public toNo:number | undefined = undefined;
  public voucherStateId:number | undefined = undefined;
  public codeVoucherGroupId:number | undefined = undefined;

  mapFrom(entity: any): GetVoucherHeadsBySpecificQuery {
    return new GetVoucherHeadsBySpecificQuery();
  }

  mapTo(): any {
    return [];
  }

  get url(): string {
    return "/accounting/VouchersHead/GetAllBySpecific";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetVoucherHeadsBySpecificQueryHandler.name)
export class GetVoucherHeadsBySpecificQueryHandler implements IRequestHandler<GetVoucherHeadsBySpecificQuery,any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {}

  async Handle(request:GetVoucherHeadsBySpecificQuery) : Promise<PaginatedList<VoucherHead>> {

    let httpRequest = new HttpRequest(request.url,request);

    return await this._httpService.Post<any,ServiceResult<PaginatedList<VoucherHead>>>(httpRequest).toPromise().then(res => {
      return res.objResult;
    });
  }
}

