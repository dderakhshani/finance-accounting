import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {Bank} from "../../../entities/bank";

export class GetBankQuery extends IRequest<GetBankQuery, Bank> {

  constructor(public entityId?: number) {
    super();
  }
  mapFrom(entity: Bank): GetBankQuery {
    return new GetBankQuery();
  }

  mapTo(): Bank {
    return new Bank();
  }

  get url(): string {
    return "/bursary/banks/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetBankQueryHandler.name)
export class GetBankQueryHandler implements IRequestHandler<GetBankQuery, Bank>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetBankQuery) : Promise<Bank> {
    let httpRequest = new HttpRequest(request.url,request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Post<any,ServiceResult<Bank>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
