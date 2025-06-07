import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Bank} from "../../../entities/bank";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetAllBanksQuery extends IRequest<GetAllBanksQuery, Bank[]>{
  mapFrom(entity: Bank[]): GetAllBanksQuery {
    return new GetAllBanksQuery();
  }

  mapTo(): Bank[] {
    return [];
  }

  get url(): string {
    return "/logistics/bank/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler(GetAllBanksQueryHandler.name)
export class GetAllBanksQueryHandler implements IRequestHandler<GetAllBanksQuery, Bank[]>{
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  async Handle(request:GetAllBanksQuery) : Promise<Bank[]> {
    let httpRequest = new HttpRequest(request.url);
    return await this.httpService.Post<any,ServiceResult<PaginatedList<Bank>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
