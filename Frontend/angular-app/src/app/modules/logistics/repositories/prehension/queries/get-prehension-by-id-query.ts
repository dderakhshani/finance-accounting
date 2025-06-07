
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {GetDriverByIdQueryHandler} from "../../driver/queries/get-driver-by-id-query";
import { Prehension } from "../../../entities/prehension";
import { ApplicationError } from "../../../../../core/exceptions/application-error";

export class GetPrehensionByIdQuery extends IRequest<GetPrehensionByIdQuery, Prehension>{
  constructor(id?:number) {
    super();
    if (id) this.id = id;
  }

  public id!:number;

  mapFrom(entity: Prehension): GetPrehensionByIdQuery {
    return new GetPrehensionByIdQuery()
  }

  mapTo(): Prehension {
    throw new ApplicationError(GetPrehensionByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/logistics/Prehension/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDriverByIdQueryHandler.name)
export class GetPrehensionByIdQueryHandler implements IRequestHandler<GetPrehensionByIdQuery, Prehension>{
  constructor(
    @Inject(HttpService) private httpService:HttpService
  ) {
  }
  Handle(request: GetPrehensionByIdQuery): Promise<Prehension> {

    let httpRequest = new HttpRequest(request.url);
    httpRequest.Query += `id=${request.id}`;
    return this.httpService.Get<ServiceResult<Prehension>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
