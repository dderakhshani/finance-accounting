import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { spReportControlsWarehouseLayoutQuantitiesByTadbir } from "../../entities/spGetDocumentItemForTadbir";



export class GetWarehouseLayoutQuantitiesByTadbirQuery extends IRequest<GetWarehouseLayoutQuantitiesByTadbirQuery, spReportControlsWarehouseLayoutQuantitiesByTadbir[]> {


  constructor(
    public warehouseId: number | undefined = undefined,
    public yearId: number | undefined = undefined,

  ) {
    super();
  }

  mapFrom(entity: spReportControlsWarehouseLayoutQuantitiesByTadbir[]): GetWarehouseLayoutQuantitiesByTadbirQuery {
    throw new ApplicationError(GetWarehouseLayoutQuantitiesByTadbirQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spReportControlsWarehouseLayoutQuantitiesByTadbir[] {
    throw new ApplicationError(GetWarehouseLayoutQuantitiesByTadbirQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetWarehouseLayoutQuantitiesByTadbir";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseLayoutQuantitiesByTadbirQueryHandler.name)
export class GetWarehouseLayoutQuantitiesByTadbirQueryHandler implements IRequestHandler<GetWarehouseLayoutQuantitiesByTadbirQuery, spReportControlsWarehouseLayoutQuantitiesByTadbir[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseLayoutQuantitiesByTadbirQuery): Promise<spReportControlsWarehouseLayoutQuantitiesByTadbir[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseLayoutQuantitiesByTadbirQuery> = new HttpRequest<GetWarehouseLayoutQuantitiesByTadbirQuery>(request.url, request);
    
    if (request.warehouseId != undefined) {
      httpRequest.Query += `&warehouseId=${request.warehouseId}`;
    }
    if (request.yearId != undefined) {
      httpRequest.Query += `&yearId=${request.yearId}`;
    }
    return await this._httpService.Get<ServiceResult<spReportControlsWarehouseLayoutQuantitiesByTadbir[]>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
