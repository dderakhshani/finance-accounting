import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { ReceiptItem } from "../../../../entities/receipt-item";
import { PaginatedList } from "../../../../../../core/models/paginated-list";
import { DocumentItemsBom } from "../../../../entities/commodity-boms";

export class GetALLDocumentItemsBomQuery extends IRequest<GetALLDocumentItemsBomQuery, PaginatedList<DocumentItemsBom>> {
  constructor(public documentItemsId: number) {
    super();
  }


  mapFrom(entity: PaginatedList<DocumentItemsBom>): GetALLDocumentItemsBomQuery {
    throw new ApplicationError(GetALLDocumentItemsBomQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<DocumentItemsBom> {
    throw new ApplicationError(GetALLDocumentItemsBomQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetALLDocumentItemsBom";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetALLDocumentItemsBomQueryHandler.name)
export class GetALLDocumentItemsBomQueryHandler implements IRequestHandler<GetALLDocumentItemsBomQuery, PaginatedList<DocumentItemsBom>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetALLDocumentItemsBomQuery): Promise<PaginatedList<DocumentItemsBom>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetALLDocumentItemsBomQuery> = new HttpRequest<GetALLDocumentItemsBomQuery>(request.url, request);
    httpRequest.Query += `documentItemsId=${request.documentItemsId}`;


    return await this._httpService.Get<ServiceResult<PaginatedList<DocumentItemsBom>>>(httpRequest).toPromise().then(response => {
     
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

    }
  
}
