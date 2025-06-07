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
import { Warehouse } from "../../../entities/warehouse";

export class GetWarehouseQuery extends IRequest<GetWarehouseQuery, any> {


   constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Warehouse): GetWarehouseQuery {
    throw new ApplicationError(GetWarehouseQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Warehouse {
    throw new ApplicationError(GetWarehouseQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Warehouse/Get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseQueryHandler.name)
export class GetWarehouseQueryHandler implements IRequestHandler<GetWarehouseQuery, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseQuery): Promise<any> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseQuery> = new HttpRequest<GetWarehouseQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Get<ServiceResult<any>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}

