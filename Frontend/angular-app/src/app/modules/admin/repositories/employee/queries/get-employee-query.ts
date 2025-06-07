import {Inject} from "@angular/core";
import {Employee} from "../../../entities/employee";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetEmployeeQuery extends IRequest<GetEmployeeQuery, Employee> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Employee): GetEmployeeQuery {
    throw new ApplicationError(GetEmployeeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Employee {
    throw new ApplicationError(GetEmployeeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/employee/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetEmployeeQueryHandler.name)
export class GetEmployeeQueryHandler implements IRequestHandler<GetEmployeeQuery, Employee> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetEmployeeQuery): Promise<Employee> {
    let httpRequest: HttpRequest<GetEmployeeQuery> = new HttpRequest<GetEmployeeQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Employee>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
