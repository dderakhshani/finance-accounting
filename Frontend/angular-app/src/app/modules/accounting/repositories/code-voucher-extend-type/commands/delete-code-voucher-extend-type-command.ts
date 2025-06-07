import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CodeVoucherExtendType} from "../../../entities/code-voucher-extend-type";

export class DeleteCodeVoucherExtendTypeCommand extends IRequest<DeleteCodeVoucherExtendTypeCommand, CodeVoucherExtendType> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: CodeVoucherExtendType): DeleteCodeVoucherExtendTypeCommand {
    throw new ApplicationError(DeleteCodeVoucherExtendTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CodeVoucherExtendType {
    throw new ApplicationError(DeleteCodeVoucherExtendTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherExtendType/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCodeVoucherExtendTypeCommandHandler.name)
export class DeleteCodeVoucherExtendTypeCommandHandler implements IRequestHandler<DeleteCodeVoucherExtendTypeCommand, CodeVoucherExtendType> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCodeVoucherExtendTypeCommand): Promise<CodeVoucherExtendType> {
    let httpRequest: HttpRequest<DeleteCodeVoucherExtendTypeCommand> = new HttpRequest<DeleteCodeVoucherExtendTypeCommand>(request.url, request);
    httpRequest.Query += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<CodeVoucherExtendType>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
