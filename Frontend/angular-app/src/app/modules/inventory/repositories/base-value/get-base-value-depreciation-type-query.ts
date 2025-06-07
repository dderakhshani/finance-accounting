import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../core/models/paginated-list";
import { BaseValueModel } from "../../entities/base-value";

export class GetDepreciationTypeBaseValueQuery extends IRequest<GetDepreciationTypeBaseValueQuery, PaginatedList<BaseValueModel>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '' ) {
    super();
  }

  mapFrom(entity: PaginatedList<BaseValueModel>): GetDepreciationTypeBaseValueQuery {
    throw new ApplicationError(GetDepreciationTypeBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<BaseValueModel> {
    throw new ApplicationError(GetDepreciationTypeBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/BaseValue/GetDepreciationTypeBaseValue";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetDepreciationTypeBaseValueQueryHandler.name)
export class GetDepreciationTypeBaseValueQueryHandler implements IRequestHandler<GetDepreciationTypeBaseValueQuery, PaginatedList<BaseValueModel>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDepreciationTypeBaseValueQuery): Promise<PaginatedList<BaseValueModel>> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetDepreciationTypeBaseValueQuery> = new HttpRequest<GetDepreciationTypeBaseValueQuery>(request.url, request);
    

    return await this._httpService.Post<GetDepreciationTypeBaseValueQuery, ServiceResult<PaginatedList<BaseValueModel>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
