import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { ApplicationError } from "../../../../core/exceptions/application-error";

import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { HttpService } from "../../../../core/services/http/http.service";
import { PaginatedList } from "../../../../core/models/paginated-list";
import { CommodityBoms } from "../../entities/commodity-boms";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";


export class GetBomsByCommodityIdQuery extends IRequest<GetBomsByCommodityIdQuery, CommodityBoms[]> {

  constructor(public CommodityId: number, public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CommodityBoms[]): GetBomsByCommodityIdQuery {
    throw new ApplicationError(GetBomsByCommodityIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityBoms[] {
    throw new ApplicationError(GetBomsByCommodityIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/CommodityBoms/GetByCommodityId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetBomsByCommodityIdQueryHandler.name)
export class GetBomsByCommodityIdQueryHandler implements IRequestHandler<GetBomsByCommodityIdQuery,  CommodityBoms[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBomsByCommodityIdQuery): Promise<CommodityBoms[]> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetBomsByCommodityIdQuery> = new HttpRequest<GetBomsByCommodityIdQuery>(request.url, request);
    

    httpRequest.Query += `CommodityId=${request.CommodityId}`;
    return await this._httpService.Get<ServiceResult<PaginatedList<CommodityBoms>>>(httpRequest).toPromise().then(response => {
     

      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })
  }
}
