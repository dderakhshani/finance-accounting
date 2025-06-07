import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { QuotaGroup } from "../../../entities/quota-group";



export class GetQuotaGroupQuery extends IRequest<GetQuotaGroupQuery, PaginatedList<QuotaGroup>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '' ) {
    super();
  }

  mapFrom(entity: PaginatedList<QuotaGroup>): GetQuotaGroupQuery {
    throw new ApplicationError(GetQuotaGroupQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<QuotaGroup> {
    throw new ApplicationError(GetQuotaGroupQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/UnitCommodityQuota/GetAllQuotaGroup";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetQuotaGroupQueryHandler.name)
export class GetQuotaGroupQueryHandler implements IRequestHandler<GetQuotaGroupQuery, PaginatedList<QuotaGroup>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetQuotaGroupQuery): Promise<PaginatedList<QuotaGroup>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetQuotaGroupQuery> = new HttpRequest<GetQuotaGroupQuery>(request.url, request);
    

    return await this._httpService.Post<GetQuotaGroupQuery, ServiceResult<PaginatedList<QuotaGroup>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
