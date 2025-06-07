import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Employee} from "../../../entities/employee";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetEmployeesQuery extends IRequest<GetEmployeesQuery, PaginatedList<Employee>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Employee>): GetEmployeesQuery {
    throw new ApplicationError(GetEmployeesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Employee> {
    throw new ApplicationError(GetEmployeesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/employee/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetEmployeesQueryHandler.name)
export class GetEmployeesQueryHandler implements IRequestHandler<GetEmployeesQuery, PaginatedList<Employee>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetEmployeesQuery): Promise<PaginatedList<Employee>> {
    let httpRequest: HttpRequest<GetEmployeesQuery> = new HttpRequest<GetEmployeesQuery>(request.url, request);

    return await this._httpService.Post<GetEmployeesQuery, ServiceResult<PaginatedList<Employee>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
