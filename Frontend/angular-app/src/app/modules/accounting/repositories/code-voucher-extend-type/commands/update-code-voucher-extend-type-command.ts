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

class CodeVoucherExtendTypeCommand {
}

export class UpdateCodeVoucherExtendTypeCommand extends IRequest<UpdateCodeVoucherExtendTypeCommand, CodeVoucherExtendType> {
  public id:number | undefined = undefined;
  public title:string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: CodeVoucherExtendType): UpdateCodeVoucherExtendTypeCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): CodeVoucherExtendType {
    throw new ApplicationError(UpdateCodeVoucherExtendTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherExtendType/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCodeVoucherExtendTypeCommandHandler.name)
export class UpdateCodeVoucherExtendTypeCommandHandler implements IRequestHandler<UpdateCodeVoucherExtendTypeCommand, CodeVoucherExtendType> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCodeVoucherExtendTypeCommand): Promise<CodeVoucherExtendType> {
    let httpRequest: HttpRequest<UpdateCodeVoucherExtendTypeCommand> = new HttpRequest<UpdateCodeVoucherExtendTypeCommand>(request.url, request);


    return await this._httpService.Put<UpdateCodeVoucherExtendTypeCommand, ServiceResult<CodeVoucherExtendType>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
