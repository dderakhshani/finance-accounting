import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {BaseValue} from "../../../entities/base-value";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteBaseValueCommand extends IRequest<DeleteBaseValueCommand, BaseValue> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: BaseValue): DeleteBaseValueCommand {
    throw new ApplicationError(DeleteBaseValueCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BaseValue {
    throw new ApplicationError(DeleteBaseValueCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/BaseValue/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteBaseValueCommandHandler.name)
export class DeleteBaseValueCommandHandler implements IRequestHandler<DeleteBaseValueCommand, BaseValue> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteBaseValueCommand): Promise<BaseValue> {
    let httpRequest: HttpRequest<DeleteBaseValueCommand> = new HttpRequest<DeleteBaseValueCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;


    return await this._httpService.Delete<ServiceResult<BaseValue>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
