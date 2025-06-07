import { HttpRequest } from "src/app/core/services/http/http-request";
import { Inject } from "@angular/core"
import { ApplicationError } from "src/app/core/exceptions/application-error"
import { ServiceResult } from "src/app/core/models/service-result"
import { HttpService } from "src/app/core/services/http/http.service"
import { MediatorHandler } from "src/app/core/services/mediator/decorator"
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces"
import { ValidationRule } from "src/app/core/validation/validation-rule"
import { NotificationService } from "src/app/shared/services/notification/notification.service"
import { FinancialAttachment } from "../../../entities/financial-attachmen"




export class CreateReceiveChequeAttachmentCommand extends IRequest<
CreateReceiveChequeAttachmentCommand,
  FinancialAttachment
> {

  public financialRequestId : number | undefined = undefined
  public attachmentId  : number | undefined = undefined
  public isVerified  : boolean | undefined = undefined
  public addressUrl : string | undefined = undefined
  public isDeleted : boolean | undefined = undefined
  public chequeSheetId : number | undefined = undefined

  constructor() {
    super();
  }

  mapFrom(entity: FinancialAttachment): CreateReceiveChequeAttachmentCommand {
    this.mapBasics(entity, this);

    return this
  }

  mapTo(): FinancialAttachment {
    throw new ApplicationError(
      CreateReceiveChequeAttachmentCommand.name,
      this.mapTo.name,
      "Not Implemented Yet"
    );
  }

  get url(): string {
    return "bursary/FinancialRequest/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateReceiveChequeAttachmentCommandHandler.name)
export class CreateReceiveChequeAttachmentCommandHandler
  implements IRequestHandler<CreateReceiveChequeAttachmentCommand, FinancialAttachment>
{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService)
    private _notificationService: NotificationService
  ) { }

  async Handle(request: CreateReceiveChequeAttachmentCommand): Promise<FinancialAttachment> {
    let httpRequest: HttpRequest<CreateReceiveChequeAttachmentCommand> =
      new HttpRequest<CreateReceiveChequeAttachmentCommand>(request.url, request);

    return await this._httpService
      .Post<CreateReceiveChequeAttachmentCommand, ServiceResult<FinancialAttachment>>(httpRequest)
      .toPromise()
      .then((response) => {
        this._notificationService.showSuccessMessage();
        return response.objResult;
      });
  }
}
