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
import { PagesCommonService } from "../../../../shared/services/pages/pages-common.service";
import { Commodity } from "../../../accounting/entities/commodity";



export class GetCommodityHaveWastQuery extends IRequest<Commodity, PaginatedList<Commodity>> {
  

  constructor(
    
    public searchTerm: string,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

 mapFrom(entity: PaginatedList<Commodity>): Commodity {
      throw new Error("Method not implemented.");
  }

  mapTo(): PaginatedList<Commodity> {
    throw new ApplicationError(GetCommodityHaveWastQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Commodity/GetCommodityHaveWast";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetCommodityHaveWastQueryHandler.name)
export class GetCommodityHaveWastQueryHandler implements IRequestHandler<GetCommodityHaveWastQuery, PaginatedList<Commodity>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService
    
  ) {
  }

  async Handle(request: GetCommodityHaveWastQuery): Promise<PaginatedList<Commodity>> {

    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetCommodityHaveWastQuery> = new HttpRequest<GetCommodityHaveWastQuery>(request.url, request);
    
    
  

    return await this._httpService.Post<GetCommodityHaveWastQuery, ServiceResult<PaginatedList<Commodity>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
 
}
