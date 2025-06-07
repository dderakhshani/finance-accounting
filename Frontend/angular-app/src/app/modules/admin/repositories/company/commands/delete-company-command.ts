import {Inject} from "@angular/core";
import {Company} from "../../../entities/company";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteCompanyCommand extends IRequest<DeleteCompanyCommand, Company> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Company): DeleteCompanyCommand {
    throw new ApplicationError(DeleteCompanyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Company {
    throw new ApplicationError(DeleteCompanyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/companyInformation/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCompanyCommandHandler.name)
export class DeleteCompanyCommandHandler implements IRequestHandler<DeleteCompanyCommand, Company> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCompanyCommand): Promise<Company> {
    let httpRequest: HttpRequest<DeleteCompanyCommand> = new HttpRequest<DeleteCompanyCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<Company>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
