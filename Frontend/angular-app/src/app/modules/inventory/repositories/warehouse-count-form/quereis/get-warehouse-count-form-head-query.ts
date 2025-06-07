import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {WarehouseCountFormHead} from "../../../entities/warehouse-count-form-head";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {GetWarehouseQuery} from "../../warehouse/queries/get-warehouse-query";
export class GetWarehouseCountFormHeadQuery extends IRequest<GetWarehouseCountFormHeadQuery, any> {

   constructor(
     public id?:number
   ) {
    super();
  }

  mapFrom(entity: WarehouseCountFormHead): GetWarehouseCountFormHeadQuery {
    throw new ApplicationError(GetWarehouseCountFormHeadQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  mapTo(): WarehouseCountFormHead {
    throw new ApplicationError(GetWarehouseCountFormHeadQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  get url(): string {
    return "/inventory/WarehouseCountForm/GetWarehouseCountFormHeadById";
  }
  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseCountFormHeadQueryHandler.name)
export class GetWarehouseCountFormHeadQueryHandler implements IRequestHandler<GetWarehouseCountFormHeadQuery, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseCountFormHeadQuery): Promise<any> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseCountFormHeadQuery> = new HttpRequest<GetWarehouseCountFormHeadQuery>(request.url, request);
    httpRequest.Query  += `id=${request.id}`;
    return await this._httpService.Get<ServiceResult<any>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}

