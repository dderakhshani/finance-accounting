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

export class UpdateAccountReferenceCommand extends IRequest<UpdateAccountReferenceCommand, AccountReference> {
  public id: string | undefined = undefined;
  public title: string | undefined = undefined;
  public code: string | undefined = undefined;
  public description: string | undefined = undefined;
  public isActive:boolean | undefined = undefined;
  public referenceGroupsId:number[] = [];
  public accountHeadIds:number[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: AccountReference): UpdateAccountReferenceCommand {
    this.mapBasics(entity, this);
    this.referenceGroupsId = entity.accountReferencesGroupsIdList;
    this.accountHeadIds = entity.personalGroupAccountHeadIds;
    return this;
  }

  mapTo(): AccountReference {
    throw new ApplicationError(UpdateAccountReferenceCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountReference/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAccountReferenceCommandHandler.name)
export class UpdateAccountReferenceCommandHandler implements IRequestHandler<UpdateAccountReferenceCommand, AccountReference> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateAccountReferenceCommand): Promise<AccountReference> {
    let httpRequest: HttpRequest<UpdateAccountReferenceCommand> = new HttpRequest<UpdateAccountReferenceCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateAccountReferenceCommand, ServiceResult<AccountReference>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
