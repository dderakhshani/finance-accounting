import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import {
  IRequest,
  IRequestHandler,
} from "../../../../../../core/services/mediator/interfaces";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { CreateChequeSheetCommand } from "../../receipt-cheque/commands/create-cheque-sheet-command";
import { FinancialRequestDetail } from "src/app/modules/bursary/entities/financial-detail";
import { CreateFinancialAttachmentCommand } from "./create-financial-attachment-command";

export class CreateReceiptCommand extends IRequest<
  CreateReceiptCommand,
  FinancialRequestDetail
> {

  public id: number | undefined = undefined;
  public documentTypeBaseId: number | undefined = undefined;
  public financialReferenceTypeBaseId: number | undefined = undefined;
  public description: string | undefined = undefined;
  public financialRequestId: number | undefined = undefined;
  public chequeSheetId: number | undefined = undefined;
  public debitAccountHeadId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public creditAccountHeadId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public currencyTypeBaseId: number | undefined = undefined;
  public isRial: boolean | undefined = undefined;
  public nonRialStatus: number | undefined = undefined;
  public amount: number | undefined = undefined;
  public currencyFee: number | undefined = undefined;
  public currencyAmount: number | undefined = undefined;
  public sowiftCode: string | undefined = undefined;
  public deliveryOrderCode: string | undefined = undefined;
  public registrationCode: string | undefined = undefined;
  public paymentCode: string | undefined = undefined;
  public chequeSheet: CreateChequeSheetCommand | undefined = undefined;
  public creditAccountReferenceCode  :string | undefined = undefined;
  public creditAccountReferenceTitle :string | undefined = undefined;
  public isCreditAccountHead:boolean | undefined = undefined;
  public isDebitAccountHead:boolean | undefined = undefined;
  public besCurrencyStatus : boolean | undefined = undefined;
  public bedCurrencyStatus : boolean | undefined = undefined;
  public isDeleted:boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: FinancialRequestDetail): CreateReceiptCommand {
    this.mapBasics(entity, this);
    this.isRial = entity.isRial;
    this.nonRialStatus = entity.nonRialStatus;
    if (entity.chequeSheet) {
      this.chequeSheet = new CreateChequeSheetCommand().mapFrom(entity.chequeSheet)
    }
    else {
      this.chequeSheet = new CreateChequeSheetCommand()
    }
    return this
  }

  mapTo(): FinancialRequestDetail {
    throw new ApplicationError(
      CreateReceiptCommand.name,
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

@MediatorHandler(CreateReceiptCommandHandler.name)
export class CreateReceiptCommandHandler
  implements IRequestHandler<CreateReceiptCommand, FinancialRequestDetail>
{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService)
    private _notificationService: NotificationService
  ) { }

  async Handle(request: CreateReceiptCommand): Promise<FinancialRequestDetail> {
    let httpRequest: HttpRequest<CreateReceiptCommand> =
      new HttpRequest<CreateReceiptCommand>(request.url, request);

    return await this._httpService
      .Post<CreateReceiptCommand, ServiceResult<FinancialRequestDetail>>(httpRequest)
      .toPromise()
      .then((response) => {
        this._notificationService.showSuccessMessage();
        return response.objResult;
      });
  }
}
