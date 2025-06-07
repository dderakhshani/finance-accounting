
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import { InvoiceItem } from "../../../../entities/invoice-item";

export class UpdateInvoiceItemCommand extends IRequest<UpdateInvoiceItemCommand, InvoiceItem> {
  public id: number | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public commoditySerial: string | undefined = undefined;
  public unitPrice: number | undefined = undefined;
  public unitBasePrice: number | undefined = undefined;
  public productionCost: number | undefined = undefined;
  public weight: number | undefined = undefined;
  public quantity: number | undefined = undefined;
  public currencyBaseId: number | undefined = undefined;
  public currencyPrice: number | undefined = undefined;
  public discount: number | undefined = undefined;
  
  public description: string | undefined = undefined;

  public documentMeasureId: number | undefined = undefined;

  public mainMeasureId: number | undefined = undefined;
  public measureUnitConversionId: number | undefined = undefined;
  public mainMeasureTitle: string | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public commodityMeasureUnitId: number | undefined = undefined;
  public secondaryQuantity: number | undefined = undefined;
  public documentHeadId: number | undefined = undefined;
  public isWrongMeasure: boolean | undefined = false;
  //-----------------------------From Invoice---------------------
  public documentNo: number | undefined = undefined;
  public requestNo: string | undefined = undefined;
  public invoiceNo: string | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public referenceId: number | undefined = undefined;

  
  
  constructor() {
    super();
  }

  mapFrom(entity: InvoiceItem): UpdateInvoiceItemCommand {
    this.mapBasics(entity, this);
    this.commodityCode = entity?.commodity?.code;
    this.commodityTitle = entity?.commodity?.title;
    this.mainMeasureTitle = entity?.commodity?.measureTitle;
    

    return this;
  }

  mapTo(): InvoiceItem {
    throw new ApplicationError(UpdateInvoiceItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/InvoiceItem/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateInvoiceItemCommandHandler.name)
export class UpdateInvoiceItemCommandHandler implements IRequestHandler<UpdateInvoiceItemCommand, InvoiceItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateInvoiceItemCommand): Promise<InvoiceItem> {
    let httpRequest: HttpRequest<UpdateInvoiceItemCommand> = new HttpRequest<UpdateInvoiceItemCommand>(request.url, request);
   

    return await this._httpService.Post<UpdateInvoiceItemCommand, ServiceResult<InvoiceItem>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    })

  }
}
