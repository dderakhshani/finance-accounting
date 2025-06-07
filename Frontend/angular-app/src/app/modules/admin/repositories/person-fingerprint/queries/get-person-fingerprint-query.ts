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

export class GetPersonFingerprintQuery extends IRequest<GetPersonFingerprintQuery, PersonFingerprint> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: PersonFingerprint): GetPersonFingerprintQuery {
    throw new ApplicationError(GetPersonFingerprintQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonFingerprint {
    throw new ApplicationError(GetPersonFingerprintQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personfingerprint/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonFingerprintQueryHandler.name)
export class GetPersonFingerprintQueryHandler implements IRequestHandler<GetPersonFingerprintQuery, PersonFingerprint> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPersonFingerprintQuery): Promise<PersonFingerprint> {
    let httpRequest: HttpRequest<GetPersonFingerprintQuery> = new HttpRequest<GetPersonFingerprintQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<PersonFingerprint>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
