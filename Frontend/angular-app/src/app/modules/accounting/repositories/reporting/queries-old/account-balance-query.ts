import {IRequest, IRequestHandler} from "src/app/core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {AccountReviewQueryHandler} from "./account-review-query";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class AccountBalanceQuery extends IRequest<AccountBalanceQuery, any[]> {

  public reportType: number | undefined = undefined;
  public level: number | undefined = undefined;
  public referenceNo: number | undefined = undefined;

  public yearId: number | undefined = undefined;
  public yearIdsJson: string | undefined = undefined;

  public voucherDateFrom: Date | undefined = undefined;
  public voucherDateTo: Date | undefined = undefined;

  public voucherNoFrom: number | undefined = undefined;
  public voucherNoTo: number | undefined = undefined;

  public documentIdFrom: number | undefined = undefined;
  public documentIdTo: number | undefined = undefined;

  public voucherDescription: string | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public voucherStateId: number | undefined = undefined;

  public debitFrom: number | undefined = undefined;
  public debitTo: number | undefined = undefined;
  public creditTo: number | undefined = undefined;
  public creditFrom: number | undefined = undefined;
  public voucherRowDescription: string | undefined = undefined;

  public transferId: string | undefined = undefined;

  public accountHeadIdsJson: string | undefined = undefined
  public referenceIdsJson: string | undefined = undefined
  public referenceGroupIdsJson: string | undefined = undefined;

  public accountHeadId:number | undefined = undefined;
  public accountReferenceId:number | undefined = undefined;
  public accountReferencesGroupId:number | undefined = undefined;

  mapFrom(entity: any): AccountBalanceQuery {
    return new AccountBalanceQuery();
  }

  mapTo(): any {
  }

  get url(): string {
    return "/accounting/reporting/GetReportBalance6";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(AccountBalanceQueryHandler.name)
export class AccountBalanceQueryHandler implements IRequestHandler<AccountBalanceQuery, any[]> {
  constructor(
    @Inject(HttpService) private httpService: HttpService
  ) {
  }

  async Handle(request: AccountBalanceQuery) {
    let httpRequest = new HttpRequest(request.url, request);
    return await this.httpService.Post<AccountBalanceQuery, ServiceResult<any[]>>(httpRequest).toPromise().then(res => {
      return res.objResult;
    })
  }
}
