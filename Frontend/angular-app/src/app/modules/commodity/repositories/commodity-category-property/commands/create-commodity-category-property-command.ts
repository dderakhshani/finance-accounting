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
  CreateCommodityCategoryPropertyItemCommand
} from "../../commodity-category-property-item/commands/create-commodity-category-property-item-command";

export class CreateCommodityCategoryPropertyCommand extends IRequest<CreateCommodityCategoryPropertyCommand, CommodityCategoryProperty> {

  public parentId: number | undefined = undefined;
  public categoryId: number | undefined = undefined;
  public levelCode: string | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public title: string | undefined = undefined;
  public measureId: number | undefined = undefined;
  public propertyTypeBaseId: number | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public items: CreateCommodityCategoryPropertyItemCommand[] = [];

  constructor() {
    super();
  }

  mapFrom(entity: CommodityCategoryProperty): CreateCommodityCategoryPropertyCommand {
    throw new ApplicationError(CreateCommodityCategoryPropertyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategoryProperty {
    throw new ApplicationError(CreateCommodityCategoryPropertyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategoryProperty/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCommodityCategoryPropertyCommandHandler.name)
export class CreateCommodityCategoryPropertyCommandHandler implements IRequestHandler<CreateCommodityCategoryPropertyCommand, CommodityCategoryProperty> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCommodityCategoryPropertyCommand): Promise<CommodityCategoryProperty> {
    let httpRequest: HttpRequest<CreateCommodityCategoryPropertyCommand> = new HttpRequest<CreateCommodityCategoryPropertyCommand>(request.url, request);
    this._notificationService.isLoader = true;
    return await this._httpService.Post<CreateCommodityCategoryPropertyCommand, ServiceResult<CommodityCategoryProperty>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
