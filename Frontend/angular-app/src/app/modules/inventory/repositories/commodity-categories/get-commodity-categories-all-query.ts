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

export class GetCommodityCategoriesALLQuery extends IRequest<GetCommodityCategoriesALLQuery, CommodityCategory[]> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CommodityCategory[]): GetCommodityCategoriesALLQuery {
    throw new ApplicationError(GetCommodityCategoriesALLQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory[] {
    throw new ApplicationError(GetCommodityCategoriesALLQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/CommodityCategories/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoriesALLQueryHandler.name)
export class GetCommodityCategoriesALLQueryHandler implements IRequestHandler<GetCommodityCategoriesALLQuery, CommodityCategory[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoriesALLQuery): Promise<CommodityCategory[]> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetCommodityCategoriesALLQuery> = new HttpRequest<GetCommodityCategoriesALLQuery>(request.url, request);
    
    return await this._httpService.Post<GetCommodityCategoriesALLQuery, ServiceResult<PaginatedList<CommodityCategory>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })
  }
}
