import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {BankAccount} from "../../../entities/bank-account";

export class GetBankAccountQuery extends IRequest<GetBankAccountQuery, BankAccount> {

  constructor(public entityId?: number) {
    super();
  }
  mapFrom(entity: BankAccount): GetBankAccountQuery {
    return new GetBankAccountQuery();
  }

  mapTo(): BankAccount {
    return new BankAccount();
  }

  get url(): string {
    return "/bursary/bankAccounts/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetBankAccountQueryHandler.name)
export class GetBankAccountQueryHandler implements IRequestHandler<GetBankAccountQuery, BankAccount>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetBankAccountQuery) : Promise<BankAccount> {
    let httpRequest = new HttpRequest(request.url,request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Post<any,ServiceResult<BankAccount>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
