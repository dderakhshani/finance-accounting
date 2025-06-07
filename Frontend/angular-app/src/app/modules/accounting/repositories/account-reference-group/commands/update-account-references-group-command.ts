import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {AccountReferencesGroup} from "../../../entities/account-references-group";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class UpdateAccountReferencesGroupCommand extends IRequest<UpdateAccountReferencesGroupCommand, AccountReferencesGroup> {
  public id: number | undefined = undefined;
  public parentTitle: string | undefined = undefined;
  public parentId: number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public isEditable:boolean | undefined = undefined;
  public code: string | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: AccountReferencesGroup): UpdateAccountReferencesGroupCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): AccountReferencesGroup {
    throw new ApplicationError(UpdateAccountReferencesGroupCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountReferencesGroup/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAccountReferencesGroupCommandHandler.name)
export class UpdateAccountReferencesGroupCommandHandler implements IRequestHandler<UpdateAccountReferencesGroupCommand, AccountReferencesGroup> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateAccountReferencesGroupCommand): Promise<AccountReferencesGroup> {
    let httpRequest: HttpRequest<UpdateAccountReferencesGroupCommand> = new HttpRequest<UpdateAccountReferencesGroupCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateAccountReferencesGroupCommand, ServiceResult<AccountReferencesGroup>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
