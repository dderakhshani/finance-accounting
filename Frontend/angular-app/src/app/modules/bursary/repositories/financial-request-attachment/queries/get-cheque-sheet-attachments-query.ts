import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {Cheque} from "../../../entities/cheque";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { FinancialAttachment } from "../../../entities/financial-attachmen";

export class GetChequeSheetAttachmentsQuery extends IRequest<GetChequeSheetAttachmentsQuery, PaginatedList<FinancialAttachment>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<FinancialAttachment>): GetChequeSheetAttachmentsQuery {
    throw new ApplicationError(GetChequeSheetAttachmentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<FinancialAttachment> {
    throw new ApplicationError(GetChequeSheetAttachmentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/FinancialRequestAttachment/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetChequeSheetAttachmentsQueryHandler.name)
export class GetChequeSheetAttachmentsQueryHandler implements IRequestHandler<GetChequeSheetAttachmentsQuery, PaginatedList<FinancialAttachment>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetChequeSheetAttachmentsQuery): Promise<PaginatedList<FinancialAttachment>> {
    let httpRequest: HttpRequest<GetChequeSheetAttachmentsQuery> = new HttpRequest<GetChequeSheetAttachmentsQuery>(request.url, request);

    return await this._httpService.Post<GetChequeSheetAttachmentsQuery, PaginatedList<FinancialAttachment>>(httpRequest).toPromise().then(response => {
      return response;
    })

  }
}
