import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PersonAddress} from "../../../entities/person-address";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeletePersonAddressCommand extends IRequest<DeletePersonAddressCommand, PersonAddress> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: PersonAddress): DeletePersonAddressCommand {
    throw new ApplicationError(DeletePersonAddressCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonAddress {
    throw new ApplicationError(DeletePersonAddressCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personAddress/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePersonAddressCommandHandler.name)
export class DeletePersonAddressCommandHandler implements IRequestHandler<DeletePersonAddressCommand, PersonAddress> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeletePersonAddressCommand): Promise<PersonAddress> {
    let httpRequest: HttpRequest<DeletePersonAddressCommand> = new HttpRequest<DeletePersonAddressCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<PersonAddress>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
