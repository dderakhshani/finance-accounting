import {Inject} from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { FinancialAttachment } from "src/app/modules/bursary/entities/financial-attachmen";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
import { NotificationService } from "src/app/shared/services/notification/notification.service";


export class GetAttachmentsQuery extends IRequest<GetAttachmentsQuery, PaginatedList<FinancialAttachment>> {

  public financialRequestId!:number;

  constructor(public id: number) {
    super();
    this.financialRequestId = id;
  }



  mapFrom(entity: PaginatedList<FinancialAttachment>): GetAttachmentsQuery {
    throw new ApplicationError(GetAttachmentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<FinancialAttachment> {
    throw new ApplicationError(GetAttachmentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/FinancialRequest/GetAttachmentsByFinancialRequestId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAttachmentsQueryHandler.name)
export class GetAttachmentsQueryHandler implements IRequestHandler<GetAttachmentsQuery, PaginatedList<FinancialAttachment>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAttachmentsQuery): Promise<PaginatedList<FinancialAttachment>> {
    let httpRequest: HttpRequest<GetAttachmentsQuery> = new HttpRequest<GetAttachmentsQuery>(request.url, request);
    httpRequest.Query  += `financialRequestId=${request.financialRequestId}`;

    return await this._httpService.Get<PaginatedList<FinancialAttachment>>(httpRequest).toPromise().then(response => {
      return response
    })
  }
}
