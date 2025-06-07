import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {AccountHead} from "../../../entities/account-head";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {AddAccountReferenceGroupToAccountHeadCommand} from "./add-account-reference-group-to-account-head-command";

export class CreateAccountHeadCommand extends IRequest<CreateAccountHeadCommand, AccountHead> {
  public codeLevel: number | undefined = undefined;
  public code: string | undefined = undefined;
  public lastLevel: boolean | undefined = undefined;
  public parentId: number | undefined = undefined;
  public parentCode: string | undefined = undefined;
  public parentTitle: string | undefined = undefined;
  public title: string | undefined = undefined;
  public description: string | undefined = undefined;
  public balanceId: number | undefined = undefined;
  public balanceBaseId: number | undefined = undefined;
  public transferId: number | undefined = undefined;
  public groupId: number | undefined = undefined;
  public currencyBaseTypeId: number | undefined = undefined;
  public currencyFlag: boolean | undefined = undefined;
  public exchengeFlag: boolean | undefined = undefined;
  public traceFlag: boolean | undefined = undefined;
  public quantityFlag: boolean | undefined = undefined;
  public isActive: boolean | undefined = undefined;
  public groups: AddAccountReferenceGroupToAccountHeadCommand[] = [];
  public accessPermission: string | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: AccountHead): CreateAccountHeadCommand {
    throw new ApplicationError(CreateAccountHeadCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccountHead {
    throw new ApplicationError(CreateAccountHeadCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountHead/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateAccountHeadCommandHandler.name)
export class CreateAccountHeadCommandHandler implements IRequestHandler<CreateAccountHeadCommand, AccountHead> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateAccountHeadCommand): Promise<AccountHead> {
    let httpRequest: HttpRequest<CreateAccountHeadCommand> = new HttpRequest<CreateAccountHeadCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateAccountHeadCommand, ServiceResult<AccountHead>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
