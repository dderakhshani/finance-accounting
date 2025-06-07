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
import {AccountHeadRelReferencesGroup} from "../../../entities/account-head-rel-references-group";

export class AddAccountReferenceGroupToAccountHeadCommand extends IRequest<AddAccountReferenceGroupToAccountHeadCommand, AccountHeadRelReferencesGroup> {
  public referenceGroupId: number | undefined = undefined;
  public referenceNo: number | undefined = undefined;



  constructor() {
    super();
  }

  mapFrom(entity: AccountHeadRelReferencesGroup): AddAccountReferenceGroupToAccountHeadCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): AccountHeadRelReferencesGroup {
    throw new ApplicationError(AddAccountReferenceGroupToAccountHeadCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(AddAccountReferenceGroupToAccountHeadCommandHandler.name)
export class AddAccountReferenceGroupToAccountHeadCommandHandler implements IRequestHandler<AddAccountReferenceGroupToAccountHeadCommand, AccountHeadRelReferencesGroup> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: AddAccountReferenceGroupToAccountHeadCommand): Promise<AccountHeadRelReferencesGroup> {
    let httpRequest: HttpRequest<AddAccountReferenceGroupToAccountHeadCommand> = new HttpRequest<AddAccountReferenceGroupToAccountHeadCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<AddAccountReferenceGroupToAccountHeadCommand, ServiceResult<AccountHeadRelReferencesGroup>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
