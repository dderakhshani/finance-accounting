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

export class GetByDocumentIdByVoucherHeadIdQuery extends IRequest<GetByDocumentIdByVoucherHeadIdQuery, Receipt> {
  constructor(public VoucherHeadId: number) {
    super();
  }


  mapFrom(entity: Receipt): GetByDocumentIdByVoucherHeadIdQuery {
    throw new ApplicationError(GetByDocumentIdByVoucherHeadIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetByDocumentIdByVoucherHeadIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetByDocumentIdByDocumentHeadId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetByDocumentIdByVoucherHeadIdQueryHandler.name)
export class GetByDocumentIdByVoucherHeadIdQueryHandler implements IRequestHandler<GetByDocumentIdByVoucherHeadIdQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetByDocumentIdByVoucherHeadIdQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetByDocumentIdByVoucherHeadIdQuery> = new HttpRequest<GetByDocumentIdByVoucherHeadIdQuery>(request.url, request);
    httpRequest.Query += `VoucherHeadId=${request.VoucherHeadId}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
