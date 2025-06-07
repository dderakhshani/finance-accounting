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
import { Data } from "@angular/router";
import { PagesCommonService } from "../../../../shared/services/pages/pages-common.service";
import { Commodity } from "../../../accounting/entities/commodity";



export class GetAllProductNeedUpdateQuery extends IRequest<Commodity, Commodity[]> {
  

  constructor() {
    super();
  }

 mapFrom(entity: Commodity[]): Commodity {
      throw new Error("Method not implemented.");
  }

  mapTo(): Commodity[] {
    throw new ApplicationError(GetAllProductNeedUpdateQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/SinaRequest/GetAllProductNeedUpdate";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAllProductNeedUpdateQueryHandler.name)
export class GetAllProductNeedUpdateQueryHandler implements IRequestHandler<GetAllProductNeedUpdateQuery, Commodity[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService
    
  ) {
  }

  async Handle(request: GetAllProductNeedUpdateQuery): Promise<Commodity[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetAllProductNeedUpdateQuery> = new HttpRequest<GetAllProductNeedUpdateQuery>(request.url, request);
   

    return await this._httpService.Get<ServiceResult<Commodity[]>>(httpRequest).toPromise().then(response => {


      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
 
}
