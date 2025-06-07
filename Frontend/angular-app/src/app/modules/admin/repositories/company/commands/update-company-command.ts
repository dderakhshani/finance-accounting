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

export class UpdateCompanyCommand extends IRequest<UpdateCompanyCommand, Company> {
  public id:number | undefined = undefined;
  public title:string | undefined = undefined;
  public uniqueName:string | undefined = undefined;
  public expireDate:Date | undefined = undefined;
  public maxNumOfUsers:number | undefined = undefined;
  public logo:string | undefined = undefined;
  public yearsId:number[] = [];

  constructor() {
    super();
  }

  mapFrom(entity: Company): UpdateCompanyCommand {
    this.mapBasics(entity,this);
    this.yearsId = entity.yearsId
    return this;
  }

  mapTo(): Company {
    throw new ApplicationError(UpdateCompanyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/companyInformation/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCompanyCommandHandler.name)
export class UpdateCompanyCommandHandler implements IRequestHandler<UpdateCompanyCommand, Company> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCompanyCommand): Promise<Company> {
    let httpRequest: HttpRequest<UpdateCompanyCommand> = new HttpRequest<UpdateCompanyCommand>(request.url, request);

    return await this._httpService.Put<UpdateCompanyCommand, ServiceResult<Company>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
