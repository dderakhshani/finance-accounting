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

export class GetPersonQuery extends IRequest<GetPersonQuery, Person> {
  constructor(public entityId: number) {
    super();
  }

  mapFrom(entity: Person): GetPersonQuery {
    throw new ApplicationError(GetPersonQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Person {
    throw new ApplicationError(GetPersonQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/person/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonQueryHandler.name)
export class GetPersonQueryHandler implements IRequestHandler<GetPersonQuery, Person> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPersonQuery): Promise<Person> {
    let httpRequest: HttpRequest<GetPersonQuery> = new HttpRequest<GetPersonQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Person>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
