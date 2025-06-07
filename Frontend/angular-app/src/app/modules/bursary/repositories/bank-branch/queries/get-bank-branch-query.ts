import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {BankBranch} from "../../../entities/bank-branch";

export class GetBankBranchQuery extends IRequest<GetBankBranchQuery, BankBranch> {

  constructor(public entityId?: number) {
    super();
  }
  mapFrom(entity: BankBranch): GetBankBranchQuery {
    return new GetBankBranchQuery();
  }

  mapTo(): BankBranch {
    return new BankBranch();
  }

  get url(): string {
    return "/bursary/bankBranches/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetBankBranchQueryHandler.name)
export class GetBankBranchQueryHandler implements IRequestHandler<GetBankBranchQuery, BankBranch>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetBankBranchQuery) : Promise<BankBranch> {
    let httpRequest = new HttpRequest(request.url,request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Post<any,ServiceResult<BankBranch>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
