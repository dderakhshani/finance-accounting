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

export class CreateAccountReferencesGroupCommand extends IRequest<CreateAccountReferencesGroupCommand, AccountReferencesGroup> {
  public parentTitle: string | undefined = undefined;
  public parentId: number | undefined = undefined;
  public companyId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public isEditable: boolean | undefined = undefined;
  public code: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: AccountReferencesGroup): CreateAccountReferencesGroupCommand {
    throw new ApplicationError(CreateAccountReferencesGroupCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccountReferencesGroup {
    throw new ApplicationError(CreateAccountReferencesGroupCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountReferencesGroup/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateAccountReferencesGroupCommandHandler.name)
export class CreateAccountReferencesGroupCommandHandler implements IRequestHandler<CreateAccountReferencesGroupCommand, AccountReferencesGroup> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateAccountReferencesGroupCommand): Promise<AccountReferencesGroup> {
    let httpRequest: HttpRequest<CreateAccountReferencesGroupCommand> = new HttpRequest<CreateAccountReferencesGroupCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateAccountReferencesGroupCommand, ServiceResult<AccountReferencesGroup>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
