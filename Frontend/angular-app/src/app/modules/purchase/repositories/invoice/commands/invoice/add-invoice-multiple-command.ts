import {Inject} from "@angular/core";

import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {invoice} from "../../../../entities/invoice";
import {AddInvoiceItemCommand} from "../invoice-item/add-invoice-item-command";


export class AddInvoiceMultipeCommand extends IRequest<AddInvoiceMultipeCommand, invoice> {

  public id: number | undefined = undefined;
  public accountReferencesGroupId: number | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public referenceId: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public documentDescription: string | undefined = undefined;
  public isManual: boolean | undefined = undefined;
  public discountPercent: number | undefined = undefined;
  public paymentTypeBaseId: number | undefined = undefined;
  public expireDate: Date | undefined = undefined;
  public documentDate: Date | undefined = new Date(new Date().setHours(0, 0, 0, 0));
  public _documentDate: string | undefined = undefined;
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

  public invoiceDocumentItems: AddInvoiceItemCommand[] = []


  public creator: string | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: invoice): AddInvoiceMultipeCommand {
    this.mapBasics(entity, this)
    this.invoiceDocumentItems = [];
    entity.items.forEach(x => {

      let mappedItem = new AddInvoiceItemCommand().mapFrom(x)
      this.invoiceDocumentItems.push(mappedItem)
    })
    return this;
  }

  mapTo(): invoice {
    throw new ApplicationError(AddInvoiceMultipeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/Invoice/AddInvoiceMultiple";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(AddInvoiceMultipeCommandHandler.name)
export class AddInvoiceMultipeCommandHandler implements IRequestHandler<AddInvoiceMultipeCommand, invoice> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddInvoiceMultipeCommand): Promise<invoice> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddInvoiceMultipeCommand> = new HttpRequest<AddInvoiceMultipeCommand>(request.url, request);


    return await this._httpService.Post<AddInvoiceMultipeCommand, ServiceResult<invoice>>(httpRequest).toPromise().then(response => {
     

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
