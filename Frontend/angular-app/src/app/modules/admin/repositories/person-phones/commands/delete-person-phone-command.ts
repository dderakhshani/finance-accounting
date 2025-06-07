import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PersonPhone} from "../../../entities/person-phone";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class DeletePersonPhoneCommand extends IRequest<DeletePersonPhoneCommand, PersonPhone> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: PersonPhone): DeletePersonPhoneCommand {
    throw new ApplicationError(DeletePersonPhoneCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonPhone {
    throw new ApplicationError(DeletePersonPhoneCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personPhones/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePersonPhoneCommandHandler.name)
export class DeletePersonPhoneCommandHandler implements IRequestHandler<DeletePersonPhoneCommand, PersonPhone> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: DeletePersonPhoneCommand): Promise<PersonPhone> {
    let httpRequest: HttpRequest<DeletePersonPhoneCommand> = new HttpRequest<DeletePersonPhoneCommand>(request.url, request);

    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<PersonPhone>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
