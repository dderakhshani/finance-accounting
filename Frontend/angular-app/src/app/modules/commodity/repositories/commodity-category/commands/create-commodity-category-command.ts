import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CommodityCategory} from "../../../entities/commodity-category";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";

export class CreateCommodityCategoryCommand extends IRequest<CreateCommodityCategoryCommand, CommodityCategory> {

  public parentId: number | undefined = undefined;
  public levelCode: string | undefined = undefined;
  public code: string | undefined = undefined;
  public codingMode: number | undefined = undefined;
  public title: string | undefined = undefined;
  public measureId: number | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public isReadOnly: boolean | undefined = undefined;
  public requireParentProduct: boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: CommodityCategory): CreateCommodityCategoryCommand {
    throw new ApplicationError(CreateCommodityCategoryCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory {
    throw new ApplicationError(CreateCommodityCategoryCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategory/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCommodityCategoryCommandHandler.name)
export class CreateCommodityCategoryCommandHandler implements IRequestHandler<CreateCommodityCategoryCommand, CommodityCategory> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCommodityCategoryCommand): Promise<CommodityCategory> {
    let httpRequest: HttpRequest<CreateCommodityCategoryCommand> = new HttpRequest<CreateCommodityCategoryCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateCommodityCategoryCommand, ServiceResult<CommodityCategory>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })


  }
}
