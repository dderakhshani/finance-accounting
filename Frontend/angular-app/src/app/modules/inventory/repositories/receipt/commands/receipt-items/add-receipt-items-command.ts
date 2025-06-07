
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

export class AddItemsCommand extends IRequest<AddItemsCommand, ReceiptItem> {
  public id: number | undefined = undefined;
  public documentHeadId: number | undefined = undefined;

  public quantity: number | undefined = undefined;
  public secondaryQuantity: number | undefined = undefined;
  public quantityChose: number | undefined = undefined;
  
 //-------------------مشخصات کالا-------------------------
  public commodityId: number | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public mainMeasureTitle: string | undefined = undefined;
  public commoditySerial: string | undefined = undefined;
  public description: string | undefined = undefined;
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
  //---------------------ثبت گروهی-----------------------
  public requestNo: string | undefined = undefined;
  public invoiceNo: string | undefined = undefined;
  public documentNo: string | undefined = undefined;
  //------------------تامین کننده -------------------
  
  public creditAccountReferenceId : number | undefined = undefined;
  public creditAccountReferenceGroupId : number | undefined = undefined;
  
  public requestDate: Date | undefined = undefined;
  public followUpReferenceId: number | undefined = undefined;
  public requesterReferenceId: number | undefined = undefined;
  public requesterReferenceTitle: string | undefined = undefined;

  //---------------مشخصات اموال
  public assets: Assets | undefined = undefined;
  public assetsSerialsCount: number | undefined = undefined;
  //---------------فرمول ساخت
  public bomValueHeaderId: number | undefined = undefined;
  //--------------درخواست خرید
  public remainQuantity: number | undefined = undefined;
  //----------اشتباه در واحد کالا
  public isWrongMeasure: boolean | undefined = false;
 
  //---------
  public weight: number | undefined = undefined;
  public selected: boolean | undefined = undefined;
 

  
  constructor() {
    super();
  }

  mapFrom(entity: ReceiptItem): AddItemsCommand {
    this.mapBasics(entity, this);
    this.commodityCode = entity?.commodity?.code;
    this.commodityTitle = entity?.commodity?.title;
    this.mainMeasureTitle = entity?.commodity?.measureTitle
    
    return this;
  }

  mapTo(): ReceiptItem {
    throw new ApplicationError(AddItemsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/receiptItem/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(AddItemsCommandHandler.name)
export class AddItemsCommandHandler implements IRequestHandler<AddItemsCommand, ReceiptItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddItemsCommand): Promise<ReceiptItem> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddItemsCommand> = new HttpRequest<AddItemsCommand>(request.url, request);
    
    return await this._httpService.Post<AddItemsCommand, ServiceResult<ReceiptItem>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult

    })

  }
}
