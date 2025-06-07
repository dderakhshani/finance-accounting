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
import { InvoiceAllStatusModel } from "../../../../entities/invoice-all-status";



export class GetInvoiceALLStatusQuery extends IRequest<GetInvoiceALLStatusQuery, PaginatedList<InvoiceAllStatusModel>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<InvoiceAllStatusModel>): GetInvoiceALLStatusQuery {
    throw new ApplicationError(GetInvoiceALLStatusQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<InvoiceAllStatusModel> {
    throw new ApplicationError(GetInvoiceALLStatusQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/invoice/GetInvoiceALLStatus";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetInvoiceALLStatusQueryHandler.name)
export class GetInvoiceALLStatusQueryHandler implements IRequestHandler<GetInvoiceALLStatusQuery, PaginatedList<InvoiceAllStatusModel>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetInvoiceALLStatusQuery): Promise<PaginatedList<InvoiceAllStatusModel>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetInvoiceALLStatusQuery> = new HttpRequest<GetInvoiceALLStatusQuery>(request.url, request);


    return await this._httpService.Post<GetInvoiceALLStatusQuery, ServiceResult<PaginatedList<InvoiceAllStatusModel>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
