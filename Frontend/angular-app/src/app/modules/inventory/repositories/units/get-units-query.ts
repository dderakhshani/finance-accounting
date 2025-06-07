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
import { Units } from "../../entities/units";



export class GetUnitsQuery extends IRequest<GetUnitsQuery, PaginatedList<Units>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '' ) {
    super();
  }

  mapFrom(entity: PaginatedList<Units>): GetUnitsQuery {
    throw new ApplicationError(GetUnitsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Units> {
    throw new ApplicationError(GetUnitsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/UnitCommodityQuota/GetAllUnits";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetUnitsQueryHandler.name)
export class GetUnitsQueryHandler implements IRequestHandler<GetUnitsQuery, PaginatedList<Units>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetUnitsQuery): Promise<PaginatedList<Units>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetUnitsQuery> = new HttpRequest<GetUnitsQuery>(request.url, request);
    

    return await this._httpService.Post<GetUnitsQuery, ServiceResult<PaginatedList<Units>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
