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

export class GetPersonAddressQuery extends IRequest<GetPersonAddressQuery, PersonAddress> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: PersonAddress): GetPersonAddressQuery {
    throw new ApplicationError(GetPersonAddressQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonAddress {
    throw new ApplicationError(GetPersonAddressQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personAddress/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonAddressQueryHandler.name)
export class GetPersonAddressQueryHandler implements IRequestHandler<GetPersonAddressQuery, PersonAddress> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPersonAddressQuery): Promise<PersonAddress> {
    let httpRequest: HttpRequest<GetPersonAddressQuery> = new HttpRequest<GetPersonAddressQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<PersonAddress>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
