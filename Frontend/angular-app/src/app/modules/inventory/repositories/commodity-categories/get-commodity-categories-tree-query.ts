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

export class GetCommodityCategoriesTreeQuery extends IRequest<GetCommodityCategoriesTreeQuery, CommodityCategory[]> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CommodityCategory[]): GetCommodityCategoriesTreeQuery {
    throw new ApplicationError(GetCommodityCategoriesTreeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory[] {
    throw new ApplicationError(GetCommodityCategoriesTreeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/CommodityCategories/GetTreeAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCommodityCategoriesTreeQueryHandler.name)
export class GetCommodityCategoriesTreeQueryHandler implements IRequestHandler<GetCommodityCategoriesTreeQuery, CommodityCategory[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCommodityCategoriesTreeQuery): Promise<CommodityCategory[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCommodityCategoriesTreeQuery> = new HttpRequest<GetCommodityCategoriesTreeQuery>(request.url, request);
    
    return await this._httpService.Post<GetCommodityCategoriesTreeQuery, ServiceResult<PaginatedList<CommodityCategory>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
