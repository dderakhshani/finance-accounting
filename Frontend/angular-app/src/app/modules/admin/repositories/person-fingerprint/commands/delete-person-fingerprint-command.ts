import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {PersonFingerprint} from "../../../entities/person-fingerprint";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeletePersonFingerprintCommand extends IRequest<DeletePersonFingerprintCommand, PersonFingerprint> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: PersonFingerprint): DeletePersonFingerprintCommand {
    throw new ApplicationError(DeletePersonFingerprintCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonFingerprint {
    throw new ApplicationError(DeletePersonFingerprintCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personfingerprint/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePersonFingerprintCommandHandler.name)
export class DeletePersonFingerprintCommandHandler implements IRequestHandler<DeletePersonFingerprintCommand, PersonFingerprint> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeletePersonFingerprintCommand): Promise<PersonFingerprint> {
    let httpRequest: HttpRequest<DeletePersonFingerprintCommand> = new HttpRequest<DeletePersonFingerprintCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<PersonFingerprint>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
