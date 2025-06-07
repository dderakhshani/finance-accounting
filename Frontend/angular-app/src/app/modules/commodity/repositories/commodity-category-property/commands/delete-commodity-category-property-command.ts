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

export class DeleteCommodityCategoryPropertyCommand extends IRequest<DeleteCommodityCategoryPropertyCommand, CommodityCategoryProperty> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: CommodityCategoryProperty): DeleteCommodityCategoryPropertyCommand {
    throw new ApplicationError(DeleteCommodityCategoryPropertyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategoryProperty {
    throw new ApplicationError(DeleteCommodityCategoryPropertyCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodityCategoryProperty/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCommodityCategoryPropertyCommandHandler.name)
export class DeleteCommodityCategoryPropertyCommandHandler implements IRequestHandler<DeleteCommodityCategoryPropertyCommand, CommodityCategoryProperty> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCommodityCategoryPropertyCommand): Promise<CommodityCategoryProperty> {
    let httpRequest: HttpRequest<DeleteCommodityCategoryPropertyCommand> = new HttpRequest<DeleteCommodityCategoryPropertyCommand>(request.url, request);
    httpRequest.Query += `id=${request.entityId}`;
    this._notificationService.isLoader = true;

    return await this._httpService.Delete<ServiceResult<CommodityCategoryProperty>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
     }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
