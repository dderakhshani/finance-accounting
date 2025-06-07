import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {FinancialRequest} from "../../../entities/financial-request";

export class GetFinancialRequestQuery extends IRequest<GetFinancialRequestQuery, FinancialRequest> {

  constructor(public entityId?: number) {
    super();
  }
  mapFrom(entity: FinancialRequest): GetFinancialRequestQuery {
    return new GetFinancialRequestQuery();
  }

  mapTo(): FinancialRequest {
    return new FinancialRequest();
  }

  get url(): string {
    return "/bursary/financialRequests/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetFinancialRequestQueryHandler.name)
export class GetFinancialRequestQueryHandler implements IRequestHandler<GetFinancialRequestQuery, FinancialRequest>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetFinancialRequestQuery) : Promise<FinancialRequest> {
    let httpRequest = new HttpRequest(request.url,request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<FinancialRequest>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
