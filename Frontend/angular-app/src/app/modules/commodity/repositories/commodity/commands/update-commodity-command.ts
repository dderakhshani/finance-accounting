import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {Commodity} from "../../../entities/commodity";
import {
  UpdateCommodityPropertyValueCommand
} from "../../commodity-property-value/commands/update-commodity-property-value-command";

export class UpdateCommodityCommand extends IRequest<UpdateCommodityCommand, Commodity> {
  public id: number | undefined = undefined;
  public commodityNationalId: string | undefined = undefined;
  public parentId: number | undefined = undefined;
  public commodityCategoryId: number | undefined = undefined;
  public levelCode: string | undefined = undefined;
  public parentCode: string | undefined = undefined;
  public code: string | undefined = undefined;
  public tadbirCode: string | undefined = undefined;
  public compactCode: string | undefined = undefined;
  public title: string | undefined = undefined;
  public descriptions: string | undefined = undefined;
  public measureId: number | undefined = undefined;
  public minimumQuantity: number | undefined = undefined;
  public maximumQuantity: number | undefined = undefined;
  public orderQuantity: number | undefined = undefined;
  public pricingTypeBaseId: number | undefined = undefined;
  public commodityNationalTitle :string | undefined = undefined;
  public isConsumable: boolean | undefined = undefined;
  public isHaveWast: boolean | undefined = undefined;
  public isAsset: boolean | undefined = undefined;

  public isHaveForceWast: boolean | undefined = undefined;
  public isActive: boolean | undefined = undefined;

  public propertyValues: UpdateCommodityPropertyValueCommand[] = [];

  constructor() {
    super();
  }

  mapFrom(entity: Commodity): UpdateCommodityCommand {
    this.mapBasics(entity,this)
    this.propertyValues = this.propertyValues.map((x:any) => new UpdateCommodityPropertyValueCommand().mapFrom(x))
    return this
  }

  mapTo(): Commodity {
    throw new ApplicationError(UpdateCommodityCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodity/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCommodityCommandHandler.name)
export class UpdateCommodityCommandHandler implements IRequestHandler<UpdateCommodityCommand, Commodity> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCommodityCommand): Promise<Commodity> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateCommodityCommand> = new HttpRequest<UpdateCommodityCommand>(request.url, request);
   
    return await this._httpService.Put<UpdateCommodityCommand, ServiceResult<Commodity>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
