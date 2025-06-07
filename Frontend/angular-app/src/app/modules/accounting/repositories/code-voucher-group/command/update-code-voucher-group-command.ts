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


export class UpdateCodeVoucherGroupCommand extends IRequest<UpdateCodeVoucherGroupCommand, CodeVoucherGroup> {
  public id:string | undefined = undefined;
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

  mapFrom(entity: CodeVoucherGroup): UpdateCodeVoucherGroupCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): CodeVoucherGroup {
    throw new ApplicationError(UpdateCodeVoucherGroupCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherGroup/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCodeVoucherGroupCommandHandler.name)
export class UpdateCodeVoucherGroupCommandHandler implements IRequestHandler<UpdateCodeVoucherGroupCommand, CodeVoucherGroup> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCodeVoucherGroupCommand): Promise<CodeVoucherGroup> {
    let httpRequest: HttpRequest<UpdateCodeVoucherGroupCommand> = new HttpRequest<UpdateCodeVoucherGroupCommand>(request.url, request);

    return await this._httpService.Put<UpdateCodeVoucherGroupCommand, ServiceResult<CodeVoucherGroup>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
