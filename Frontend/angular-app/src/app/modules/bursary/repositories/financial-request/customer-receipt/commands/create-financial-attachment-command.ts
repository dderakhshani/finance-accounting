import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { FinancialAttachment } from "src/app/modules/bursary/entities/financial-attachmen";
import { NotificationService } from "src/app/shared/services/notification/notification.service";

export class CreateFinancialAttachmentCommand extends IRequest<
  CreateFinancialAttachmentCommand,
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

  mapFrom(entity: FinancialAttachment): CreateFinancialAttachmentCommand {
    this.mapBasics(entity, this);

    return this
  }

  mapTo(): FinancialAttachment {
    throw new ApplicationError(
      CreateFinancialAttachmentCommand.name,
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

@MediatorHandler(CreateFinancialAttachmentCommandHandler.name)
export class CreateFinancialAttachmentCommandHandler
  implements IRequestHandler<CreateFinancialAttachmentCommand, FinancialAttachment>
{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService)
    private _notificationService: NotificationService
  ) { }

  async Handle(request: CreateFinancialAttachmentCommand): Promise<FinancialAttachment> {
    let httpRequest: HttpRequest<CreateFinancialAttachmentCommand> =
      new HttpRequest<CreateFinancialAttachmentCommand>(request.url, request);

    return await this._httpService
      .Post<CreateFinancialAttachmentCommand, ServiceResult<FinancialAttachment>>(httpRequest)
      .toPromise()
      .then((response) => {
        this._notificationService.showSuccessMessage();
        return response.objResult;
      });
  }
}
