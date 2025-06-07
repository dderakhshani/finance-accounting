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

export class DeleteEmployeeCommand extends IRequest<DeleteEmployeeCommand, Employee> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Employee): DeleteEmployeeCommand {
    throw new ApplicationError(DeleteEmployeeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Employee {
    throw new ApplicationError(DeleteEmployeeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/employee/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteEmployeeCommandHandler.name)
export class DeleteEmployeeCommandHandler implements IRequestHandler<DeleteEmployeeCommand, Employee> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteEmployeeCommand): Promise<Employee> {
    let httpRequest: HttpRequest<DeleteEmployeeCommand> = new HttpRequest<DeleteEmployeeCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<Employee>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
