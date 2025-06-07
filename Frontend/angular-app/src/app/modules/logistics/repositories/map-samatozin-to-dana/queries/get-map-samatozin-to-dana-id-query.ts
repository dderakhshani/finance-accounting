
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {GetDriverByIdQueryHandler} from "../../driver/queries/get-driver-by-id-query";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MapSamatozinToDana } from "../../../entities/map-samatozin-to-dana";

export class GetMapSamatozinToDanadQuery extends IRequest<GetMapSamatozinToDanadQuery, MapSamatozinToDana>{
  constructor(id?:number) {
    super();
    if (id) this.id = id;
  }

  public id!:number;

  mapFrom(entity: MapSamatozinToDana): GetMapSamatozinToDanadQuery {
    return new GetMapSamatozinToDanadQuery()
  }

  mapTo(): MapSamatozinToDana {
    throw new ApplicationError(GetMapSamatozinToDanadQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/logistics/MapSamatozinToDana/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDriverByIdQueryHandler.name)
export class GetMapSamatozinToDanadQueryHandler implements IRequestHandler<GetMapSamatozinToDanadQuery, MapSamatozinToDana>{
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  Handle(request: GetMapSamatozinToDanadQuery): Promise<MapSamatozinToDana> {

    let httpRequest = new HttpRequest(request.url);
    httpRequest.Query += `id=${request.id}`;
    return this.httpService.Get<ServiceResult<MapSamatozinToDana>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
