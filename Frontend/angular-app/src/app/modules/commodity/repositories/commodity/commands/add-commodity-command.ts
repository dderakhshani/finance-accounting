import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Commodity} from "../../../entities/commodity";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {
  AddCommodityPropertyValueCommand
} from "../../commodity-property-value/commands/add-commodity-property-value-command";
import { UpdateCommodityPropertyValueCommand } from "../../commodity-property-value/commands/update-commodity-property-value-command";

export class AddCommodityCommand extends IRequest<AddCommodityCommand, Commodity> {
  public parentId: number | undefined = undefined;
  public commodityNationalId: string | undefined = undefined;
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
  public commodityNationalTitle: string | undefined = undefined;
  public isHaveWast: boolean  = true;
  public isActive: boolean = true;
  public isHaveForceWast: boolean | undefined = undefined;
  public isAsset: boolean | undefined = undefined;
  public isConsumable: boolean | undefined = undefined;
  public propertyValues: AddCommodityPropertyValueCommand[] = [];

  constructor() {
    super();
  }

  mapFrom(entity: Commodity): AddCommodityCommand {
    this.mapBasics(entity, this)
    this.propertyValues = this.propertyValues.map((x: any) => new UpdateCommodityPropertyValueCommand().mapFrom(x))
    return this
  }

  mapTo(): Commodity {
    throw new ApplicationError(AddCommodityCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodity/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(AddCommodityCommandHandler.name)
export class AddCommodityCommandHandler implements IRequestHandler<AddCommodityCommand, Commodity> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: AddCommodityCommand): Promise<Commodity> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<AddCommodityCommand> = new HttpRequest<AddCommodityCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<AddCommodityCommand, ServiceResult<Commodity>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
