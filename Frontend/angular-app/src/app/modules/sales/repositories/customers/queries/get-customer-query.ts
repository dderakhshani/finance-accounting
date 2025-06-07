import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Customer} from "../../../entities/Customer";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCustomerQuery extends IRequest<GetCustomerQuery, Customer> {
  constructor(public entityId: number) {
    super();
  }

  mapFrom(entity: Customer): GetCustomerQuery {
    throw new ApplicationError(GetCustomerQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Customer {
    throw new ApplicationError(GetCustomerQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/sale/customers/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCustomerQueryHandler.name)
export class GetCustomerQueryHandler implements IRequestHandler<GetCustomerQuery, Customer> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetCustomerQuery): Promise<Customer> {
    let httpRequest: HttpRequest<GetCustomerQuery> = new HttpRequest<GetCustomerQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Customer>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
