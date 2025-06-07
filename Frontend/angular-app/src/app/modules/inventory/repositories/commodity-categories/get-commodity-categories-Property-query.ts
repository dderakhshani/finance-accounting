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

import { CommodityCategoryProperty } from "src/app/modules/commodity/entities/commodity-category-property";

export class GetInverntoryCommodityCategoriesPropertyQuery extends IRequest<GetInverntoryCommodityCategoriesPropertyQuery, CommodityCategoryProperty[]> {

  constructor(public entityId?:number,public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CommodityCategoryProperty[]): GetInverntoryCommodityCategoriesPropertyQuery {
    throw new ApplicationError(GetInverntoryCommodityCategoriesPropertyQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategoryProperty[] {
    throw new ApplicationError(GetInverntoryCommodityCategoriesPropertyQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayoutProperty/GetPropertyByWarehouseLayoutId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetInverntoryCommodityCategoriesPropertyQueryHandler.name)
export class GetInverntoryCommodityCategoriesPropertyQueryHandler implements IRequestHandler<GetInverntoryCommodityCategoriesPropertyQuery, CommodityCategoryProperty[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetInverntoryCommodityCategoriesPropertyQuery): Promise<CommodityCategoryProperty[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetInverntoryCommodityCategoriesPropertyQuery> = new HttpRequest<GetInverntoryCommodityCategoriesPropertyQuery>(request.url, request);
    httpRequest.Query  += `WarehouseLayoutId=${request.entityId}`;

    return await this._httpService.Post<GetInverntoryCommodityCategoriesPropertyQuery, ServiceResult<PaginatedList<CommodityCategoryProperty>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
