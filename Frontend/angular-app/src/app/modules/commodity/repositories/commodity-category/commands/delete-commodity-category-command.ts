import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CommodityCategory} from "../../../entities/commodity-category";

export class DeleteCommodityCategoryCommand extends IRequest<DeleteCommodityCategoryCommand, CommodityCategory> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: CommodityCategory): DeleteCommodityCategoryCommand {
    throw new ApplicationError(DeleteCommodityCategoryCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory {
    throw new ApplicationError(DeleteCommodityCategoryCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategory/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCommodityCategoryCommandHandler.name)
export class DeleteCommodityCategoryCommandHandler implements IRequestHandler<DeleteCommodityCategoryCommand, CommodityCategory> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCommodityCategoryCommand): Promise<CommodityCategory> {
    let httpRequest: HttpRequest<DeleteCommodityCategoryCommand> = new HttpRequest<DeleteCommodityCategoryCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<CommodityCategory>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
