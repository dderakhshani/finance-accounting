import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Person} from "../../../entities/person";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeletePersonCommand extends IRequest<DeletePersonCommand, Person> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Person): DeletePersonCommand {
    throw new ApplicationError(DeletePersonCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Person {
    throw new ApplicationError(DeletePersonCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/person/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePersonCommandHandler.name)
export class DeletePersonCommandHandler implements IRequestHandler<DeletePersonCommand, Person> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeletePersonCommand): Promise<Person> {
    let httpRequest: HttpRequest<DeletePersonCommand> = new HttpRequest<DeletePersonCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

      return await this._httpService.Delete<ServiceResult<Person>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
