import {Usance} from "../../../entities/usance";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {GetDriverByIdQueryHandler} from "../../driver/queries/get-driver-by-id-query";

export class GetUsanceByIdQuery extends IRequest<GetUsanceByIdQuery, Usance>{
  constructor(id?:number) {
    super();
    if (id) this.id = id;
  }

  public id!:number;

  mapFrom(entity: Usance): GetUsanceByIdQuery {
    return new GetUsanceByIdQuery()
  }

  mapTo(): Usance {
    return new Usance();
  }

  get url(): string {
    return "/logistics/usance/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDriverByIdQueryHandler.name)
export class GetUsanceByIdQueryHandler implements IRequestHandler<GetUsanceByIdQuery, Usance>{
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  Handle(request: GetUsanceByIdQuery): Promise<Usance> {

    let httpRequest = new HttpRequest(request.url);
    httpRequest.Query += `id=${request.id}`;
    return this.httpService.Get<ServiceResult<Usance>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
