
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import { Assets } from "../../../../entities/Assets";
import { ReceiptItem } from "../../../../entities/receipt-item";

export class AddItemsProductCommand extends IRequest<AddItemsProductCommand, ReceiptItem> {
  public id: number | undefined = undefined;
  
  public quantity: number | undefined = undefined;
  
 //-------------------مشخصات کالا-------------------------
  public commodityId: number | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public mainMeasureTitle: string | undefined = undefined;
  public description: string | undefined = undefined;
  public requestNo: number | undefined = undefined;
  public bomValueHeaderId: number | undefined = undefined;
  
  //---------------------قیمت کالا-------------------------
  public unitPrice: number | undefined = undefined;
  public unitBasePrice: number | undefined = undefined;
  public productionCost: number | undefined = undefined;
  public currencyBaseId: number | undefined = undefined;
  public currencyPrice: number | undefined = undefined;
  public discount: number | undefined = undefined;
  //------------------واحد کالا----------------------------
  public documentMeasureId: number | undefined = undefined;
  public mainMeasureId: number | undefined = undefined;
  public measureUnitConversionId: number | undefined = undefined;
  public commodityMeasureUnitId: number | undefined = undefined;
  //=====================================================
  
  public warehouseId: number | undefined = undefined;
  public selected: boolean | undefined = undefined;
 

  
  constructor() {
    super();
  }

  mapFrom(entity: ReceiptItem): AddItemsProductCommand {
    this.mapBasics(entity, this);
    this.commodityCode = entity?.commodity?.code;
    this.commodityTitle = entity?.commodity?.title;
    this.mainMeasureTitle = entity?.commodity?.measureTitle
    
    return this;
  }

  mapTo(): ReceiptItem {
    throw new ApplicationError(AddItemsProductCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/receiptItem/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(AddItemsProductCommandHandler.name)
export class AddItemsProductCommandHandler implements IRequestHandler<AddItemsProductCommand, ReceiptItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddItemsProductCommand): Promise<ReceiptItem> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddItemsProductCommand> = new HttpRequest<AddItemsProductCommand>(request.url, request);
    
    return await this._httpService.Post<AddItemsProductCommand, ServiceResult<ReceiptItem>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult

    })

  }
}
