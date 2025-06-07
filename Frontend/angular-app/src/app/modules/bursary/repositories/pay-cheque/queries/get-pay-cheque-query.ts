import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PayCheque} from "../../../entities/pay-cheque";


export class GetPayChequeQuery extends IRequest<GetPayChequeQuery, PayCheque> {

  constructor(public entityId?: number) {
    super();
  }
  mapFrom(entity: PayCheque): GetPayChequeQuery {
    return new GetPayChequeQuery();
  }

  mapTo(): PayCheque {
    return new PayCheque();
  }

  get url(): string {
    return "/bursary/payCheques/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetPayChequeQueryHandler.name)
export class GetPayChequeQueryHandler implements IRequestHandler<GetPayChequeQuery, PayCheque>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetPayChequeQuery) : Promise<PayCheque> {
    let httpRequest = new HttpRequest(request.url,request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Post<any,ServiceResult<PayCheque>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
