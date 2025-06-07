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

export class GetCategoriesCodeProductQuery extends IRequest<GetCategoriesCodeProductQuery, CommodityCategory[]> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: CommodityCategory[]): GetCategoriesCodeProductQuery {
    throw new ApplicationError(GetCategoriesCodeProductQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CommodityCategory[] {
    throw new ApplicationError(GetCategoriesCodeProductQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/BaseValue/GetCommodityGroupBaseValue";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCategoriesCodeProductQueryHandler.name)
export class GetCategoriesCodeProductQueryHandler implements IRequestHandler<GetCategoriesCodeProductQuery, CommodityCategory[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  

  async Handle(request: GetCategoriesCodeProductQuery): Promise<CommodityCategory[]> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetCategoriesCodeProductQuery> = new HttpRequest<GetCategoriesCodeProductQuery>(request.url, request);
    

    return await this._httpService.Post<GetCategoriesCodeProductQuery, ServiceResult<PaginatedList<CommodityCategory>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult.data
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })
  }
}
