import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Driver} from "../../../entities/driver";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class GetDriverByIdQuery extends IRequest<GetDriverByIdQuery, Driver>{
  constructor(id?:number) {
    super();
    if (id) this.id = id;
  }
  public id!:number;

  mapFrom(entity: Driver): GetDriverByIdQuery {
    return new GetDriverByIdQuery();
  }

  mapTo(): Driver {
    return new Driver();
  }

  get url(): string {
    return "/logistics/driver/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDriverByIdQueryHandler.name)
export class GetDriverByIdQueryHandler implements IRequestHandler<GetDriverByIdQuery, Driver>{
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  Handle(request: GetDriverByIdQuery): Promise<Driver> {

    let httpRequest = new HttpRequest(request.url);
    httpRequest.Query += `id=${request.id}`;
    return this.httpService.Get<ServiceResult<Driver>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }

}
