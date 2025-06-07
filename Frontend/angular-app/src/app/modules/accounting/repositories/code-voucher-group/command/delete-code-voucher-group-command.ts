import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CodeVoucherGroup} from "../../../entities/code-voucher-group";

export class DeleteCodeVoucherGroupCommand extends IRequest<DeleteCodeVoucherGroupCommand, CodeVoucherGroup> {

  constructor(public id: any) {
    super();
  }

  mapFrom(entity: CodeVoucherGroup): DeleteCodeVoucherGroupCommand {
    throw new ApplicationError(DeleteCodeVoucherGroupCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CodeVoucherGroup {
    throw new ApplicationError(DeleteCodeVoucherGroupCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherGroup/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCodeVoucherGroupCommandHandler.name)
export class DeleteCodeVoucherGroupCommandHandler implements IRequestHandler<DeleteCodeVoucherGroupCommand, CodeVoucherGroup> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCodeVoucherGroupCommand): Promise<CodeVoucherGroup> {
    let httpRequest: HttpRequest<DeleteCodeVoucherGroupCommand> = new HttpRequest<DeleteCodeVoucherGroupCommand>(request.url, request);
    httpRequest.Query  += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<CodeVoucherGroup>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
