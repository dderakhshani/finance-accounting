import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {FinancialRequestDetail} from "../../../entities/financial-request-detail";


export class GetFinancialRequestDetailQuery extends IRequest<GetFinancialRequestDetailQuery, FinancialRequestDetail> {

  constructor(public entityId?: number) {
    super();
  }
  mapFrom(entity: FinancialRequestDetail): GetFinancialRequestDetailQuery {
    return new GetFinancialRequestDetailQuery();
  }

  mapTo(): FinancialRequestDetail {
    return new FinancialRequestDetail();
  }

  get url(): string {
    return "/bursary/financialRequestDetails/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetFinancialRequestDetailQueryHandler.name)
export class GetFinancialRequestDetailQueryHandler implements IRequestHandler<GetFinancialRequestDetailQuery, FinancialRequestDetail>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetFinancialRequestDetailQuery) : Promise<FinancialRequestDetail> {
    let httpRequest = new HttpRequest(request.url,request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<FinancialRequestDetail>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
