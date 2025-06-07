import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {CommodityCategoryProperty} from "../../../entities/commodity-category-property";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {
  UpdateCommodityCategoryPropertyItemCommand
} from "../../commodity-category-property-item/commands/update-commodity-category-property-item-command";

export class UpdateCommodityCategoryPropertyCommand extends IRequest<UpdateCommodityCategoryPropertyCommand, CommodityCategoryProperty> {
  public id: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public categoryId: number | undefined = undefined;
  public levelCode: string | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public title: string | undefined = undefined;
  public measureId: number | undefined = undefined;
  public propertyTypeBaseId: number | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public items: UpdateCommodityCategoryPropertyItemCommand[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: CommodityCategoryProperty): UpdateCommodityCategoryPropertyCommand {
    this.mapBasics(entity,this);
    this.items = [];
    entity.items.forEach(item => {
      this.items.push(new UpdateCommodityCategoryPropertyItemCommand().mapFrom(item))
    })
    return  this;
  }

  mapTo(): CommodityCategoryProperty {
    throw new ApplicationError(UpdateCommodityCategoryPropertyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/CommodityCategoryProperty/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCommodityCategoryPropertyCommandHandler.name)


export class UpdateCommodityCategoryPropertyCommandHandler implements IRequestHandler<UpdateCommodityCategoryPropertyCommand, CommodityCategoryProperty> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCommodityCategoryPropertyCommand): Promise<CommodityCategoryProperty> {
    let httpRequest: HttpRequest<UpdateCommodityCategoryPropertyCommand> = new HttpRequest<UpdateCommodityCategoryPropertyCommand>(request.url, request);
    this._notificationService.isLoader = true;
    return await this._httpService.Post<UpdateCommodityCategoryPropertyCommand, ServiceResult<CommodityCategoryProperty>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}



