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

export class CreateEmployeeCommand extends IRequest<CreateEmployeeCommand, Employee> {

  public personId:number | undefined = undefined;
  public unitPositionId:number | undefined = undefined;
  public employmentDate:Date | undefined = undefined;
  public leaveDate:Date | undefined = undefined;
  public employeeCode:string | undefined = undefined;

  public unitId:number | undefined = undefined;
  public branchId:number | undefined = undefined;
  public accountReferenceCode:string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Employee): CreateEmployeeCommand {
    throw new ApplicationError(CreateEmployeeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Employee {
    throw new ApplicationError(CreateEmployeeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/employee/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateEmployeeCommandHandler.name)
export class CreateEmployeeCommandHandler implements IRequestHandler<CreateEmployeeCommand, Employee> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateEmployeeCommand): Promise<Employee> {
    let httpRequest: HttpRequest<CreateEmployeeCommand> = new HttpRequest<CreateEmployeeCommand>(request.url, request);

    return await this._httpService.Post<CreateEmployeeCommand, ServiceResult<Employee>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
