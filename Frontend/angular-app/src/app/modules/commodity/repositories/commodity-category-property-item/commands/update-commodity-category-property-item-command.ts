import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {CommodityCategoryPropertyItem} from "../../../entities/commodity-category-property-item";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {EntityStates} from "../../../../../core/enums/entity-states";

export class UpdateCommodityCategoryPropertyItemCommand extends IRequest<UpdateCommodityCategoryPropertyItemCommand, CommodityCategoryPropertyItem> {
  public id: number | undefined = undefined;
  public categoryPropertyId: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public code: string | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public isActive: boolean | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: CommodityCategoryPropertyItem): UpdateCommodityCategoryPropertyItemCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): CommodityCategoryPropertyItem {
    throw new ApplicationError(UpdateCommodityCategoryPropertyItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategoryPropertyItem/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCommodityCategoryPropertyItemCommandHandler.name)
export class UpdateCommodityCategoryPropertyItemCommandHandler implements IRequestHandler<UpdateCommodityCategoryPropertyItemCommand, CommodityCategoryPropertyItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCommodityCategoryPropertyItemCommand): Promise<CommodityCategoryPropertyItem> {
    let httpRequest: HttpRequest<UpdateCommodityCategoryPropertyItemCommand> = new HttpRequest<UpdateCommodityCategoryPropertyItemCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateCommodityCategoryPropertyItemCommand, ServiceResult<CommodityCategoryPropertyItem>>(httpRequest).toPromise().then(response => {
      // this._notificationService.showHttpSuccessMessage()
      return response.objResult
    })
  }
}
