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

export class DeleteCommodityCategoryPropertyItemCommand extends IRequest<DeleteCommodityCategoryPropertyItemCommand, CommodityCategoryPropertyItem> {

  constructor() {
    super();
  }

  mapFrom(entity: CommodityCategoryPropertyItem): DeleteCommodityCategoryPropertyItemCommand {
    throw new ApplicationError(DeleteCommodityCategoryPropertyItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategoryPropertyItem {
    throw new ApplicationError(DeleteCommodityCategoryPropertyItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategoryPropertyItem/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCommodityCategoryPropertyItemCommandHandler.name)
export class DeleteCommodityCategoryPropertyItemCommandHandler implements IRequestHandler<DeleteCommodityCategoryPropertyItemCommand, CommodityCategoryPropertyItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCommodityCategoryPropertyItemCommand): Promise<CommodityCategoryPropertyItem> {
    let httpRequest: HttpRequest<DeleteCommodityCategoryPropertyItemCommand> = new HttpRequest<DeleteCommodityCategoryPropertyItemCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Delete<ServiceResult<CommodityCategoryPropertyItem>>(httpRequest).toPromise().then(response => {
      // this._notificationService.showHttpSuccessMessage()
      return response.objResult
    })
  }
}
