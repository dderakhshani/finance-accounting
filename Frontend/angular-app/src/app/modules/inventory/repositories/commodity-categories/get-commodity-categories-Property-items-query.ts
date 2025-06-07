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

import { PaginatedList } from "../../../../core/models/paginated-list";
import { CommodityCategoryPropertyItem } from "src/app/modules/commodity/entities/commodity-category-property-item";


export class GetInverntoryCommodityCategoriesPropertyItemsQuery extends IRequest<GetInverntoryCommodityCategoriesPropertyItemsQuery, CommodityCategoryPropertyItem[]> {

  constructor(public entityId?:number,public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CommodityCategoryPropertyItem[]): GetInverntoryCommodityCategoriesPropertyItemsQuery {
    throw new ApplicationError(GetInverntoryCommodityCategoriesPropertyItemsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategoryPropertyItem[] {
    throw new ApplicationError(GetInverntoryCommodityCategoriesPropertyItemsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayoutPropertyItems/GetPropertyItemsByWarehouseLayoutId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetInverntoryCommodityCategoriesPropertyItemsQueryHandler.name)
export class GetInverntoryCommodityCategoriesPropertyItemsQueryHandler implements IRequestHandler<GetInverntoryCommodityCategoriesPropertyItemsQuery, CommodityCategoryPropertyItem[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetInverntoryCommodityCategoriesPropertyItemsQuery): Promise<CommodityCategoryPropertyItem[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetInverntoryCommodityCategoriesPropertyItemsQuery> = new HttpRequest<GetInverntoryCommodityCategoriesPropertyItemsQuery>(request.url, request);
    httpRequest.Query  += `WarehouseLayoutId=${request.entityId}`;

    return await this._httpService.Post<GetInverntoryCommodityCategoriesPropertyItemsQuery, ServiceResult<PaginatedList<CommodityCategoryPropertyItem>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
