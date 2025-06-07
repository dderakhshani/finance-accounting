import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Bank} from "../../../entities/bank";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";

export class GetBankByIdQuery extends IRequest<GetBankByIdQuery, Bank>{
  constructor(id?:number) {
    super();
    if (id) this.id = id;
  }
  public id!:number;

  mapFrom(entity: Bank): GetBankByIdQuery {
    return new GetBankByIdQuery();
  }

  mapTo(): Bank {
    return new Bank();
  }

  get url(): string {
    return "/logistics/bank/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler(GetBankByIdQueryHandler.name)
export class GetBankByIdQueryHandler implements IRequestHandler<GetBankByIdQuery, Bank>{
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  Handle(request: GetBankByIdQuery): Promise<Bank> {

    let httpRequest = new HttpRequest(request.url);
    httpRequest.Query += `id=${request.id}`;
    return this.httpService.Get<ServiceResult<Bank>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }

}
