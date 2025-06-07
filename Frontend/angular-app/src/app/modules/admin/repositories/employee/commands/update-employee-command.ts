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

export class UpdateEmployeeCommand extends IRequest<UpdateEmployeeCommand, Employee> {

  public id:number | undefined = undefined;
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

  mapFrom(entity: Employee): UpdateEmployeeCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): Employee {
    throw new ApplicationError(UpdateEmployeeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/employee/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateEmployeeCommandHandler.name)
export class UpdateEmployeeCommandHandler implements IRequestHandler<UpdateEmployeeCommand, Employee> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateEmployeeCommand): Promise<Employee> {
    let httpRequest: HttpRequest<UpdateEmployeeCommand> = new HttpRequest<UpdateEmployeeCommand>(request.url, request);

    return await this._httpService.Put<UpdateEmployeeCommand, ServiceResult<Employee>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
