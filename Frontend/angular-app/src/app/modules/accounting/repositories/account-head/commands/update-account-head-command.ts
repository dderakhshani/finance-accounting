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

export class UpdateAccountHeadCommand extends IRequest<UpdateAccountHeadCommand, AccountHead> {
  public id:number | undefined = undefined;
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
  public accessPermission:string | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: AccountHead): UpdateAccountHeadCommand {
    this.mapBasics(entity, this);
    entity.accountHeadRelReferenceGroups.forEach(x => {
      this.groups.push(new AddAccountReferenceGroupToAccountHeadCommand().mapFrom(x))
    })
    return this;
  }

  mapTo(): AccountHead {
    throw new ApplicationError(UpdateAccountHeadCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountHead/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAccountHeadCommandHandler.name)
export class UpdateAccountHeadCommandHandler implements IRequestHandler<UpdateAccountHeadCommand, AccountHead> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateAccountHeadCommand): Promise<AccountHead> {
    let httpRequest: HttpRequest<UpdateAccountHeadCommand> = new HttpRequest<UpdateAccountHeadCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateAccountHeadCommand, ServiceResult<AccountHead>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
