import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";

import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../../core/models/paginated-list";
import { BaseValueModel } from "../../../../entities/base-value";



export class GetReceiptALLBaseValueQuery extends IRequest<GetReceiptALLBaseValueQuery, PaginatedList<BaseValueModel>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<BaseValueModel>): GetReceiptALLBaseValueQuery {
    throw new ApplicationError(GetReceiptALLBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<BaseValueModel> {
    throw new ApplicationError(GetReceiptALLBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/BaseValue/GetReceiptALLBaseValue";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetReceiptALLBaseValueQueryHandler.name)
export class GetReceiptALLBaseValueQueryHandler implements IRequestHandler<GetReceiptALLBaseValueQuery, PaginatedList<BaseValueModel>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetReceiptALLBaseValueQuery): Promise<PaginatedList<BaseValueModel>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetReceiptALLBaseValueQuery> = new HttpRequest<GetReceiptALLBaseValueQuery>(request.url, request);


    return await this._httpService.Post<GetReceiptALLBaseValueQuery, ServiceResult<PaginatedList<BaseValueModel>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
