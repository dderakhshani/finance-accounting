import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PersonCustomer} from "../../../entities/person-customer";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class DeletePersonCustomerCommand extends IRequest<DeletePersonCustomerCommand, PersonCustomer> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: PersonCustomer): DeletePersonCustomerCommand {
    throw new ApplicationError(DeletePersonCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonCustomer {
    throw new ApplicationError(DeletePersonCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personCustomer/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePersonCustomerCommandHandler.name)
export class DeletePersonCustomerCommandHandler implements IRequestHandler<DeletePersonCustomerCommand, PersonCustomer> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: DeletePersonCustomerCommand): Promise<PersonCustomer> {
    let httpRequest: HttpRequest<DeletePersonCustomerCommand> = new HttpRequest<DeletePersonCustomerCommand>(request.url, request);

    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<PersonCustomer>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
