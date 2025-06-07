import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { Receipt } from "../../../../entities/receipt";

export class GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery extends IRequest<GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery, Receipt> {
  constructor(public DocumentNo: number, public codeVoucherGroupId: number ) {
    super();
  }


  mapFrom(entity: Receipt): GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery {
    throw new ApplicationError(GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetByDocumentNoAndCodeVoucherGroupId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetByDocumentNoAndDocumentCodeVoucherGroupIdQueryHandler.name)
export class GetByDocumentNoAndDocumentCodeVoucherGroupIdQueryHandler implements IRequestHandler<GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery> = new HttpRequest<GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery>(request.url, request);
    httpRequest.Query += `DocumentNo=${request.DocumentNo}&codeVoucherGroupId=${request.codeVoucherGroupId}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
