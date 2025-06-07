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

export class CreateCompanyCommand extends IRequest<CreateCompanyCommand, Company> {

  public title:string | undefined = undefined;
  public uniqueName:string | undefined = undefined;
  public expireDate:Date | undefined = undefined;
  public maxNumOfUsers:number | undefined = undefined;
  public logo:string | undefined = undefined;
  public yearsId:number[] = [];

  constructor() {
    super();
  }

  mapFrom(entity: Company): CreateCompanyCommand {
    throw new ApplicationError(CreateCompanyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Company {
    throw new ApplicationError(CreateCompanyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/companyInformation/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCompanyCommandHandler.name)
export class CreateCompanyCommandHandler implements IRequestHandler<CreateCompanyCommand, Company> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCompanyCommand): Promise<Company> {
    let httpRequest: HttpRequest<CreateCompanyCommand> = new HttpRequest<CreateCompanyCommand>(request.url, request);

    return await this._httpService.Post<CreateCompanyCommand, ServiceResult<Company>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
