import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {CommodityPropertyValue} from "../../../entities/commodity-property-value";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class UpdateCommodityPropertyValueCommand extends IRequest<UpdateCommodityPropertyValueCommand, CommodityPropertyValue> {

  public id: number | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public categoryId: number | undefined = undefined;
  public categoryPropertyId: number | undefined = undefined;
  public valuePropertyItemId: number | undefined = undefined;
  public value: string | undefined = undefined;
  public title: string | undefined = undefined;
  public propertyTypeBaseId: number | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: CommodityPropertyValue): UpdateCommodityPropertyValueCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): CommodityPropertyValue {
    throw new ApplicationError(UpdateCommodityPropertyValueCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityPropertyValue/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCommodityPropertyValueCommandHandler.name)
export class UpdateCommodityPropertyValueCommandHandler implements IRequestHandler<UpdateCommodityPropertyValueCommand, CommodityPropertyValue> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCommodityPropertyValueCommand): Promise<CommodityPropertyValue> {
    let httpRequest: HttpRequest<UpdateCommodityPropertyValueCommand> = new HttpRequest<UpdateCommodityPropertyValueCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()


    return await this._httpService.Put<UpdateCommodityPropertyValueCommand, ServiceResult<CommodityPropertyValue>>(httpRequest).toPromise().then(response => {
      // this._notificationService.showHttpSuccessMessage()
      return response.objResult
    })
  }
}
