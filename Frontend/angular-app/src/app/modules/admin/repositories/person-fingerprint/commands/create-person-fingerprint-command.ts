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

export class CreatePersonFingerprintCommand extends IRequest<CreatePersonFingerprintCommand, PersonFingerprint> {
  public personId:number | undefined = undefined;
  public fingerBaseId:number | undefined = undefined;
  public fingerPrintPhotoUrl:string | undefined = undefined;
  public fingerprintTemplate:string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: PersonFingerprint): CreatePersonFingerprintCommand {
    throw new ApplicationError(CreatePersonFingerprintCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonFingerprint {
    throw new ApplicationError(CreatePersonFingerprintCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personfingerprint/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePersonFingerprintCommandHandler.name)
export class CreatePersonFingerprintCommandHandler implements IRequestHandler<CreatePersonFingerprintCommand, PersonFingerprint> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreatePersonFingerprintCommand): Promise<PersonFingerprint> {
    let httpRequest: HttpRequest<CreatePersonFingerprintCommand> = new HttpRequest<CreatePersonFingerprintCommand>(request.url, request);

    return await this._httpService.Post<CreatePersonFingerprintCommand, ServiceResult<PersonFingerprint>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
