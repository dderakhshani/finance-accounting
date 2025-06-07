import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { HttpService } from "../../../../core/services/http/http.service";
import { PaginatedList } from "../../../../core/models/paginated-list";
import { CommodityCategory } from "src/app/modules/commodity/entities/commodity-category";

export class GetInverntoryCommodityCategoriesQuery extends IRequest<GetInverntoryCommodityCategoriesQuery, CommodityCategory[]> {

  constructor(public entityId?:number,public warhoseId?:number,public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CommodityCategory[]): GetInverntoryCommodityCategoriesQuery {
    throw new ApplicationError(GetInverntoryCommodityCategoriesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory[] {
    throw new ApplicationError(GetInverntoryCommodityCategoriesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayoutCategories/GetCategores";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetInverntoryCommodityCategoriesQueryHandler.name)
export class GetInverntoryCommodityCategoriesQueryHandler implements IRequestHandler<GetInverntoryCommodityCategoriesQuery, CommodityCategory[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetInverntoryCommodityCategoriesQuery): Promise<CommodityCategory[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetInverntoryCommodityCategoriesQuery> = new HttpRequest<GetInverntoryCommodityCategoriesQuery>(request.url, request);
    if(request.entityId==undefined)
    {
      request.entityId=0;
    }

    httpRequest.Query  += `ParentId=${request.entityId}&warhoseId=${request.warhoseId}`;

    return await this._httpService.Post<GetInverntoryCommodityCategoriesQuery, ServiceResult<PaginatedList<CommodityCategory>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
