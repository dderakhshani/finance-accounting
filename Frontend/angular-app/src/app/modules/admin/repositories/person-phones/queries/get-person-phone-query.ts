import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PersonPhone} from "../../../entities/person-phone";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetPersonPhoneQuery extends IRequest<GetPersonPhoneQuery, PersonPhone> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: PersonPhone): GetPersonPhoneQuery {
    throw new ApplicationError(GetPersonPhoneQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonPhone {
    throw new ApplicationError(GetPersonPhoneQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personPhones/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonPhoneQueryHandler.name)
export class GetPersonPhoneQueryHandler implements IRequestHandler<GetPersonPhoneQuery, PersonPhone> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetPersonPhoneQuery): Promise<PersonPhone> {
    let httpRequest: HttpRequest<GetPersonPhoneQuery> = new HttpRequest<GetPersonPhoneQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<PersonPhone>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
