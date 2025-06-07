import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Driver} from "../../../entities/driver";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetAllDriversQuery extends IRequest<GetAllDriversQuery,Driver[]>{
  mapFrom(entity: Driver[]): GetAllDriversQuery {
    return new GetAllDriversQuery();
  }

  mapTo(): Driver[] {
    return [];
  }

  get url(): string {
    return "/logistics/driver/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAllDriversQueryHandler.name)
export class GetAllDriversQueryHandler implements IRequestHandler<GetAllDriversQuery, Driver[]>{
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  async Handle(request:GetAllDriversQuery) : Promise<Driver[]> {
    let httpRequest = new HttpRequest(request.url);
    return await this.httpService.Post<any,ServiceResult<PaginatedList<Driver>>>(httpRequest).toPromise().then(res => {
      return res.objResult.data
    })
  }
}
