import { Inject } from "@angular/core";
import { invoice } from "../../../../entities/invoice";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { AddInvoiceItemCommand } from "../invoice-item/add-invoice-item-command";
import { Utilities } from "../../../../../../core/utilities";


export class AddInvoiceCommand extends IRequest<AddInvoiceCommand, invoice> {

  public id: number | undefined = undefined;
 
  public codeVoucherGroupId: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public documentDescription: string | undefined = undefined;
  public isManual: boolean | undefined = undefined;
  public discountPercent: number | undefined = undefined;
  public paymentTypeBaseId: number | undefined = undefined;
  public expireDate: Date | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public documentNo: string | undefined = undefined;
  public serialFormulaBaseValueUniquename: number | undefined = undefined;
  public partNumber: string | undefined = undefined;
  public requestNumber: number | undefined = undefined;
  public requesterReferenceId: number | undefined = undefined;
  public followUpReferenceId: number | undefined = undefined;
  public requesterReferenceTitle: string | undefined = undefined;
  public followUpReferenceTitle: string | undefined = undefined;
  public invoiceNo: string | undefined = undefined;
  public tags: string | undefined = undefined;
  public requestNo: number | undefined = undefined;
  public vatDutiesTax: number | undefined = undefined;
  public totalProductionCost: number | undefined = undefined;
  public totalItemPrice: number | undefined = undefined;
  public invoiceDocumentItems: AddInvoiceItemCommand[] = []

  public attachmentIds: number[] = []

 
  constructor() {
    super();
  }

  mapFrom(entity: invoice): AddInvoiceCommand {

    this.mapBasics(entity, this)
    this.invoiceDocumentItems = [];
    entity.items.forEach(x => {

      let mappedItem = new AddInvoiceItemCommand().mapFrom(x)
      this.invoiceDocumentItems.push(mappedItem)
    })
    return this;
  }

  mapTo(): invoice {
    throw new ApplicationError(AddInvoiceCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/invoice/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(AddInvoiceCommandHandler.name)
export class AddInvoiceCommandHandler implements IRequestHandler<AddInvoiceCommand, invoice> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddInvoiceCommand): Promise<invoice> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddInvoiceCommand> = new HttpRequest<AddInvoiceCommand>(request.url, request);


    return await this._httpService.Post<AddInvoiceCommand, ServiceResult<invoice>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
