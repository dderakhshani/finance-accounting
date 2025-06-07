import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {WarehouseLayoutCategory} from "../../../../entities/warehouse-layout";

export class DeleteWarehouseLayoutCategoryCommand extends IRequest<DeleteWarehouseLayoutCategoryCommand, WarehouseLayoutCategory> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: WarehouseLayoutCategory): DeleteWarehouseLayoutCategoryCommand {
    throw new ApplicationError(DeleteWarehouseLayoutCategoryCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): WarehouseLayoutCategory {
    throw new ApplicationError(DeleteWarehouseLayoutCategoryCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/WarehouseLayoutCategories/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteWarehouseLayoutCategoryCommandHandler.name)
export class DeleteWarehouseLayoutCategoryCommandHandler implements IRequestHandler<DeleteWarehouseLayoutCategoryCommand, WarehouseLayoutCategory> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteWarehouseLayoutCategoryCommand): Promise<WarehouseLayoutCategory> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<DeleteWarehouseLayoutCategoryCommand> = new HttpRequest<DeleteWarehouseLayoutCategoryCommand>(request.url, request);

    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<WarehouseLayoutCategory>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
