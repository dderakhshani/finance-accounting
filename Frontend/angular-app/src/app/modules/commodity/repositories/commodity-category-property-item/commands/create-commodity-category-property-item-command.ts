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

export class CreateCommodityCategoryPropertyItemCommand extends IRequest<CreateCommodityCategoryPropertyItemCommand, CommodityCategoryPropertyItem> {
  public state: number = EntityStates.Inserted;
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

  mapFrom(entity: CommodityCategoryPropertyItem): CreateCommodityCategoryPropertyItemCommand {
    throw new ApplicationError(CreateCommodityCategoryPropertyItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategoryPropertyItem {
    throw new ApplicationError(CreateCommodityCategoryPropertyItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategoryPropertyItem/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCommodityCategoryPropertyItemCommandHandler.name)
export class CreateCommodityCategoryPropertyItemCommandHandler implements IRequestHandler<CreateCommodityCategoryPropertyItemCommand, CommodityCategoryPropertyItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCommodityCategoryPropertyItemCommand): Promise<CommodityCategoryPropertyItem> {
    let httpRequest: HttpRequest<CreateCommodityCategoryPropertyItemCommand> = new HttpRequest<CreateCommodityCategoryPropertyItemCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateCommodityCategoryPropertyItemCommand, ServiceResult<CommodityCategoryPropertyItem>>(httpRequest).toPromise().then(response => {
      // this._notificationService.showHttpSuccessMessage()
      return response.objResult
    })
  }
}
