import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Usance} from "../../../entities/usance";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetAllUsanceQuery extends IRequest<GetAllUsanceQuery, Usance[]> {
  mapFrom(entity: Usance[]): GetAllUsanceQuery {
    return new GetAllUsanceQuery();
  }

  mapTo(): Usance[] {
    return [];
  }

  get url(): string {
    return "/logistics/usance/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAllUsanceQueryHandler.name)
export class GetAllUsanceQueryHandler implements IRequestHandler<GetAllUsanceQuery, Usance[]>{
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  async Handle(request:GetAllUsanceQuery) : Promise<Usance[]> {
    let httpRequest = new HttpRequest(request.url);
    return await this.httpService.Post<any,ServiceResult<PaginatedList<Usance>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
