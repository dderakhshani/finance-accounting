import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {WarehouseLayout} from "../../../entities/warehouse-layout";

export class GetParentIdAllChildByCapacityAvailabeQuery extends IRequest<GetParentIdAllChildByCapacityAvailabeQuery, PaginatedList<WarehouseLayout>> {

  constructor(public entityId: number) {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseLayout>): GetParentIdAllChildByCapacityAvailabeQuery {
    throw new ApplicationError(GetParentIdAllChildByCapacityAvailabeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseLayout> {
    throw new ApplicationError(GetParentIdAllChildByCapacityAvailabeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayout/GetParentIdAllChildByCapacityAvailabe";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetParentIdAllChildByCapacityAvailabeQueryHandler.name)
export class GetParentIdAllChildByCapacityAvailabeQueryHandler implements IRequestHandler<GetParentIdAllChildByCapacityAvailabeQuery, PaginatedList<WarehouseLayout>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetParentIdAllChildByCapacityAvailabeQuery): Promise<PaginatedList<WarehouseLayout>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetParentIdAllChildByCapacityAvailabeQuery> = new HttpRequest<GetParentIdAllChildByCapacityAvailabeQuery>(request.url, request);

    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Post<GetParentIdAllChildByCapacityAvailabeQuery, ServiceResult<PaginatedList<WarehouseLayout>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
