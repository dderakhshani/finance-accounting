import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { VoucherHead } from "../../../entities/voucher-head";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";
import { HttpService } from "../../../../../core/services/http/http.service";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "../../../../../core/models/service-result";
import { UpdateVouchersHeadCommand } from "../commands/update-vouchers-head-command";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { ApplicationError } from "../../../../../core/exceptions/application-error";

export class GetVoucherHeadQuery extends IRequest<GetVoucherHeadQuery, any>{
  constructor(id: number, includeVoucherDetails: boolean = false, public isPrint: boolean = false, public selectedVoucherDetailIds: number[] = [], public printType: number = 0) {
    super();
    this.id = id;
    this.includeVoucherDetails = includeVoucherDetails;
  }
  public id!: number;
  public includeVoucherDetails!: boolean;

  mapFrom(entity: any): GetVoucherHeadQuery {
    throw new ApplicationError(GetVoucherHeadQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(GetVoucherHeadQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/VouchersHead/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
@MediatorHandler(GetVoucherHeadQueryHandler.name)
export class GetVoucherHeadQueryHandler implements IRequestHandler<GetVoucherHeadQuery, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) { }

  async Handle(request: GetVoucherHeadQuery): Promise<any> {
    let httpRequest = new HttpRequest<GetVoucherHeadQuery>(request.url, request);
    httpRequest.Query += `id=${request.id}&includeVoucherDetails=${request.includeVoucherDetails}&IsPrint=${request.isPrint}&selectedVoucherDetailIds=${request.selectedVoucherDetailIds}&printType=${request.printType}`;

    return await this._httpService.Get<ServiceResult<any>>(httpRequest).toPromise().then(res => {
      return res.objResult
    });

  }
}

