import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {CodeVoucherGroup} from "../../../entities/code-voucher-group";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreateCodeVoucherGroupCommand extends IRequest<CreateCodeVoucherGroupCommand, CodeVoucherGroup> {
  public title:string | undefined = undefined;
  public code:string | undefined = undefined;
  public lastEditableDate:Date | undefined = undefined;
  public blankDateFormula:string | undefined = undefined;
  public extendTypeId:number | undefined = undefined;
  public isAuto:boolean | undefined = undefined;
  public isEditable:boolean | undefined = undefined;
  public isActive:boolean | undefined = undefined;
  public uniqueName:string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: CodeVoucherGroup): CreateCodeVoucherGroupCommand {
    throw new ApplicationError(CreateCodeVoucherGroupCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CodeVoucherGroup {
    throw new ApplicationError(CreateCodeVoucherGroupCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherGroup/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCodeVoucherGroupCommandHandler.name)
export class CreateCodeVoucherGroupCommandHandler implements IRequestHandler<CreateCodeVoucherGroupCommand, CodeVoucherGroup> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCodeVoucherGroupCommand): Promise<CodeVoucherGroup> {
    let httpRequest: HttpRequest<CreateCodeVoucherGroupCommand> = new HttpRequest<CreateCodeVoucherGroupCommand>(request.url, request);

    return await this._httpService.Post<CreateCodeVoucherGroupCommand, ServiceResult<CodeVoucherGroup>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
