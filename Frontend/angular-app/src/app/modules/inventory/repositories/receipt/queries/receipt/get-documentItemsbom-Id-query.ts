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

export class GetDocumentItemsBomByIdQuery extends IRequest<GetDocumentItemsBomByIdQuery, ReceiptItem> {
  constructor(public Id: number) {
    super();
  }


  mapFrom(entity: ReceiptItem): GetDocumentItemsBomByIdQuery {
    throw new ApplicationError(GetDocumentItemsBomByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ReceiptItem {
    throw new ApplicationError(GetDocumentItemsBomByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetDocumentItemsBomById";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetDocumentItemsBomByIdQueryHandler.name)
export class GetDocumentItemsBomByIdQueryHandler implements IRequestHandler<GetDocumentItemsBomByIdQuery, ReceiptItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDocumentItemsBomByIdQuery): Promise<ReceiptItem> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetDocumentItemsBomByIdQuery> = new HttpRequest<GetDocumentItemsBomByIdQuery>(request.url, request);
    httpRequest.Query += `Id=${request.Id}`;


    return await this._httpService.Get<ServiceResult<ReceiptItem>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    
  }).finally(() => {
    this._notificationService.isLoader = false;
  })
  }
}
