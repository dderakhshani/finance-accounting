import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PersonCustomer} from "../../../entities/person-customer";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetPersonCustomerQuery extends IRequest<GetPersonCustomerQuery, PersonCustomer> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: PersonCustomer): GetPersonCustomerQuery {
    throw new ApplicationError(GetPersonCustomerQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonCustomer {
    throw new ApplicationError(GetPersonCustomerQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personCustomer/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonCustomerQueryHandler.name)
export class GetPersonCustomerQueryHandler implements IRequestHandler<GetPersonCustomerQuery, PersonCustomer> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetPersonCustomerQuery): Promise<PersonCustomer> {
    let httpRequest: HttpRequest<GetPersonCustomerQuery> = new HttpRequest<GetPersonCustomerQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<PersonCustomer>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
