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

export class UpdatePersonFingerprintCommand extends IRequest<UpdatePersonFingerprintCommand, PersonFingerprint> {
  public id:number | undefined = undefined;
  public personId:number | undefined = undefined;
  public fingerBaseId:number | undefined = undefined;
  public fingerPrintPhotoUrl:string | undefined = undefined;
  public fingerprintTemplate:string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: PersonFingerprint): UpdatePersonFingerprintCommand {
    throw new ApplicationError(UpdatePersonFingerprintCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonFingerprint {
    throw new ApplicationError(UpdatePersonFingerprintCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personfingerprint/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePersonFingerprintCommandHandler.name)
export class UpdatePersonFingerprintCommandHandler implements IRequestHandler<UpdatePersonFingerprintCommand, PersonFingerprint> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdatePersonFingerprintCommand): Promise<PersonFingerprint> {
    let httpRequest: HttpRequest<UpdatePersonFingerprintCommand> = new HttpRequest<UpdatePersonFingerprintCommand>(request.url, request);

    return await this._httpService.Put<UpdatePersonFingerprintCommand, ServiceResult<PersonFingerprint>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
