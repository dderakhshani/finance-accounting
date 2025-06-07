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
import { BaseValueModel } from "../../entities/base-value";


export class GetCurrencyBaseValueQuery extends IRequest<GetCurrencyBaseValueQuery, PaginatedList<BaseValueModel>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '' ) {
    super();
  }

  mapFrom(entity: PaginatedList<BaseValueModel>): GetCurrencyBaseValueQuery {
    throw new ApplicationError(GetCurrencyBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<BaseValueModel> {
    throw new ApplicationError(GetCurrencyBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/BaseValue/GetCurrencyBaseValue";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetCurrencyBaseValueQueryHandler.name)
export class GetCurrencyBaseValueQueryHandler implements IRequestHandler<GetCurrencyBaseValueQuery, PaginatedList<BaseValueModel>> {

  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCurrencyBaseValueQuery): Promise<PaginatedList<BaseValueModel>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCurrencyBaseValueQuery> = new HttpRequest<GetCurrencyBaseValueQuery>(request.url, request);
    

    return await this._httpService.Post<GetCurrencyBaseValueQuery, ServiceResult<PaginatedList<BaseValueModel>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
