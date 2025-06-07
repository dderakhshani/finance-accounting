import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class AccountReviewQuery extends IRequest<AccountReviewQuery,any[]>{

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

  mapFrom(entity: any): AccountReviewQuery {
    return new AccountReviewQuery();
  }

  mapTo(): any {
    return undefined;
  }

  get url(): string {
    return "/accounting/reporting/GetAccountReview";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(AccountReviewQueryHandler.name)
export class AccountReviewQueryHandler implements IRequestHandler<AccountReviewQuery,any[]> {
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  async Handle(request:AccountReviewQuery) : Promise<any[]> {
    let httpRequest = new HttpRequest(request.url,request);
    return await this.httpService.Post<AccountReviewQuery,ServiceResult<any[]>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }

}
