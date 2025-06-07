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
import { ReceiptAllStatusModel } from "src/app/modules/inventory/entities/receipt-all-status";


export class GetALLCodeVoucherGroupsQuery extends IRequest<GetALLCodeVoucherGroupsQuery, PaginatedList<ReceiptAllStatusModel>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<ReceiptAllStatusModel>): GetALLCodeVoucherGroupsQuery {
    throw new ApplicationError(GetALLCodeVoucherGroupsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<ReceiptAllStatusModel> {
    throw new ApplicationError(GetALLCodeVoucherGroupsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/CodeVoucherGroups/GetALL";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetALLCodeVoucherGroupsQueryHandler.name)
export class GetALLCodeVoucherGroupsQueryHandler implements IRequestHandler<GetALLCodeVoucherGroupsQuery, PaginatedList<ReceiptAllStatusModel>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetALLCodeVoucherGroupsQuery): Promise<PaginatedList<ReceiptAllStatusModel>> {
    this._notificationService.isLoaderDropdown = true;
    let httpRequest: HttpRequest<GetALLCodeVoucherGroupsQuery> = new HttpRequest<GetALLCodeVoucherGroupsQuery>(request.url, request);
    

    return await this._httpService.Post<GetALLCodeVoucherGroupsQuery, ServiceResult<PaginatedList<ReceiptAllStatusModel>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoaderDropdown = false;
    })

  }
}
