import {ReceiptItem} from "../../../../entities/receipt-item";
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
export class UpdateTemporaryReceiptItemCommand extends IRequest<UpdateTemporaryReceiptItemCommand, ReceiptItem> {
  public id: number | undefined = undefined;
  public commodityId: number | undefined = undefined;
  
  public unitPrice: number | undefined = undefined;
  public unitBasePrice: number | undefined = undefined;
  public productionCost: number | undefined = undefined;
  public weight: number | undefined = undefined;
  public quantity: number | undefined = undefined;
  public remainQuantity: number | undefined = undefined;
  public currencyBaseId: number | undefined = undefined;
  public currencyPrice: number | undefined = undefined;
  public sumCurrencyPrice: number | undefined = undefined;
  public unitPriceWithExtraCost: number | undefined = undefined;
  public addCurrencyPrice: number | undefined = undefined;

  public currencyRate: number | undefined = undefined;
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
  public hasPermissionEditQuantity: boolean | undefined = undefined;//اجازه ویرایش تعداد را دارد
  public isFreightChargePaid: boolean | undefined = undefined;
  //-----------------------------From Receipt---------------------
  public documentNo: number | undefined = undefined;
  public requestNo: string | undefined = undefined;
  public invoiceNo: string | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  //------------------تامین کننده --------------------------------
  public creditAccountReferenceId : number | undefined = undefined;
  public creditAccountReferenceGroupId : number | undefined = undefined;

  
  
  //----------اگر درحالت ورود شماره سریال باشیم
  
  public commoditySerial: string | undefined = undefined;
  public assetsSerialsCount: number | undefined = undefined;
  //---------------مشخصات اموال
  public assets: Assets | undefined = undefined;
  
  constructor() {
    super();
  }

  mapFrom(entity: ReceiptItem): UpdateTemporaryReceiptItemCommand {
    this.mapBasics(entity, this);
    this.commodityCode = entity?.commodity?.code;
    this.commodityTitle = entity?.commodity?.title;
    this.mainMeasureTitle = entity?.commodity?.measureTitle;
    

    return this;
  }

  mapTo(): ReceiptItem {
    throw new ApplicationError(UpdateTemporaryReceiptItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/receiptItem/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateTemporaryReceiptItemCommandHandler.name)
export class UpdateTemporaryReceiptItemCommandHandler implements IRequestHandler<UpdateTemporaryReceiptItemCommand, ReceiptItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateTemporaryReceiptItemCommand): Promise<ReceiptItem> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateTemporaryReceiptItemCommand> = new HttpRequest<UpdateTemporaryReceiptItemCommand>(request.url, request);
   

    return await this._httpService.Post<UpdateTemporaryReceiptItemCommand, ServiceResult<ReceiptItem>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult
    })

  }
}
