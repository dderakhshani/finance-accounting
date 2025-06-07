import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Customer} from "../../../entities/Customer";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteCustomerCommand extends IRequest<DeleteCustomerCommand, Customer> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Customer): DeleteCustomerCommand {
    throw new ApplicationError(DeleteCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Customer {
    throw new ApplicationError(DeleteCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/sale/customers/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCustomerCommandHandler.name)
export class DeleteCustomerCommandHandler implements IRequestHandler<DeleteCustomerCommand, Customer> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: DeleteCustomerCommand): Promise<Customer> {
    let httpRequest: HttpRequest<DeleteCustomerCommand> = new HttpRequest<DeleteCustomerCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<Customer>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
