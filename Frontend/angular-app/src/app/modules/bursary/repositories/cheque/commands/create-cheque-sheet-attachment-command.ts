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

export class CreateChequeSheetAttachmentCommand extends IRequest<
CreateChequeSheetAttachmentCommand,
  FinancialAttachment
> {

  public attachments :FinancialAttachment[] =[];

  constructor() {
    super();
  }

  mapFrom(entity: FinancialAttachment): CreateChequeSheetAttachmentCommand {
    this.mapBasics(entity, this);

    return this
  }

  mapTo(): FinancialAttachment {
    throw new ApplicationError(
      CreateChequeSheetAttachmentCommand.name,
      this.mapTo.name,
      "Not Implemented Yet"
    );
  }

  get url(): string {
    return "/bursary/FinancialRequestAttachment/SetAttachmentForChequeSheet";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateChequeSheetAttachmentCommandHandler.name)
export class CreateChequeSheetAttachmentCommandHandler
  implements IRequestHandler<CreateChequeSheetAttachmentCommand, FinancialAttachment>
{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService)
    private _notificationService: NotificationService
  ) { }

  async Handle(request: CreateChequeSheetAttachmentCommand): Promise<FinancialAttachment> {
    let httpRequest: HttpRequest<CreateChequeSheetAttachmentCommand> =
      new HttpRequest<CreateChequeSheetAttachmentCommand>(request.url, request);

    return await this._httpService
      .Post<CreateChequeSheetAttachmentCommand, ServiceResult<FinancialAttachment>>(httpRequest)
      .toPromise()
      .then((response) => {
        this._notificationService.showSuccessMessage();
        return response.objResult;
      });
  }
}
