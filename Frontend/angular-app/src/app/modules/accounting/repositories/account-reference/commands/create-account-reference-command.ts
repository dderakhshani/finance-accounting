import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {AccountReference} from "../../../entities/account-reference";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class CreateAccountReferenceCommand extends IRequest<CreateAccountReferenceCommand, AccountReference> {

  public title: string | undefined = undefined;
  public code: string | undefined = undefined;
  public description: string | undefined = undefined;
  public isActive:boolean | undefined = undefined;
  public accountReferenceTypeBaseId:number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: AccountReference): CreateAccountReferenceCommand {
    throw new ApplicationError(CreateAccountReferenceCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccountReference {
    throw new ApplicationError(CreateAccountReferenceCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountReference/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateAccountReferenceCommandHandler.name)
export class CreateAccountReferenceCommandHandler implements IRequestHandler<CreateAccountReferenceCommand, AccountReference> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateAccountReferenceCommand): Promise<AccountReference> {
    let httpRequest: HttpRequest<CreateAccountReferenceCommand> = new HttpRequest<CreateAccountReferenceCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateAccountReferenceCommand, ServiceResult<AccountReference>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
