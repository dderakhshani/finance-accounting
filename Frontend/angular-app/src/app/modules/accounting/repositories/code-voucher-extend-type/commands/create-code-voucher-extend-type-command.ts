import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CodeVoucherExtendType} from "../../../entities/code-voucher-extend-type";

export class CreateCodeVoucherExtendTypeCommand extends IRequest<CreateCodeVoucherExtendTypeCommand, CodeVoucherExtendType> {
  public title:string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: CodeVoucherExtendType): CreateCodeVoucherExtendTypeCommand {
    throw new ApplicationError(CreateCodeVoucherExtendTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CodeVoucherExtendType {
    throw new ApplicationError(CreateCodeVoucherExtendTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/codeVoucherExtendType/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCodeVoucherExtendTypeCommandHandler.name)
export class CreateCodeVoucherExtendTypeCommandHandler implements IRequestHandler<CreateCodeVoucherExtendTypeCommand, CodeVoucherExtendType> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCodeVoucherExtendTypeCommand): Promise<CodeVoucherExtendType> {
    let httpRequest: HttpRequest<CreateCodeVoucherExtendTypeCommand> = new HttpRequest<CreateCodeVoucherExtendTypeCommand>(request.url, request);

    return await this._httpService.Post<CreateCodeVoucherExtendTypeCommand, ServiceResult<CodeVoucherExtendType>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })


  }
}
