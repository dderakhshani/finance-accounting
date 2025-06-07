import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { Help } from "../../../entities/help";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { Inject } from "@angular/core";
import { HttpService } from "../../../../../core/services/http/http.service";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "../../../../../core/models/service-result";

export class GetHelpQuery extends IRequest<GetHelpQuery, Help> {
  constructor(public entityId: number) {
    super();
  }

  mapFrom(entity: Help): GetHelpQuery {
    throw new ApplicationError(GetHelpQuery.name, this.mapTo.name,'Not Implemented Yet')
  }

  mapTo(): Help {
    throw new ApplicationError(GetHelpQuery.name, this.mapTo.name,'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/help/get"
  }

  get validationRules() : ValidationRule[]{
    return [];
  }
}

@MediatorHandler(GetHelpQueryHandler.name)
export class GetHelpQueryHandler implements IRequestHandler<GetHelpQuery, Help> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetHelpQuery) : Promise<Help>{
    let httpRequest: HttpRequest<GetHelpQuery> = new HttpRequest<GetHelpQuery>(request.url, request)
        httpRequest.Query = `id=${request.entityId}`
    return await this._httpService.Get<ServiceResult<Help>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}

