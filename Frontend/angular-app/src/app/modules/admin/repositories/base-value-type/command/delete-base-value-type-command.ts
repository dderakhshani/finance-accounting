import {Inject} from "@angular/core";
import {BaseValueType} from "../../../entities/base-value-type";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteBaseValueTypeCommand extends IRequest<DeleteBaseValueTypeCommand, BaseValueType> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: BaseValueType): DeleteBaseValueTypeCommand {
    throw new ApplicationError(DeleteBaseValueTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BaseValueType {
    throw new ApplicationError(DeleteBaseValueTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/BaseValueType/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteBaseValueTypeCommandHandler.name)
export class DeleteBaseValueTypeCommandHandler implements IRequestHandler<DeleteBaseValueTypeCommand, BaseValueType> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteBaseValueTypeCommand): Promise<BaseValueType> {
    let httpRequest: HttpRequest<DeleteBaseValueTypeCommand> = new HttpRequest<DeleteBaseValueTypeCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Delete<ServiceResult<BaseValueType>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
