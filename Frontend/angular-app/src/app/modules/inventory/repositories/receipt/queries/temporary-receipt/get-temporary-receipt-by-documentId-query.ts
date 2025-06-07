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

export class GetTemporaryRecepitByDocumentNoQuery extends IRequest<GetTemporaryRecepitByDocumentNoQuery, Receipt> {
  constructor(
    public documentNo: number,
    
    
  ) {
    super();
  }


  mapFrom(entity: Receipt): GetTemporaryRecepitByDocumentNoQuery {
    throw new ApplicationError(GetTemporaryRecepitByDocumentNoQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetTemporaryRecepitByDocumentNoQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/TemporaryReceipt/GetByDocumentNo";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetTemporaryRecepitByDocumentNoQueryHandler.name)
export class GetTemporaryRecepitByDocumentNoQueryHandler implements IRequestHandler<GetTemporaryRecepitByDocumentNoQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetTemporaryRecepitByDocumentNoQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetTemporaryRecepitByDocumentNoQuery> = new HttpRequest<GetTemporaryRecepitByDocumentNoQuery>(request.url, request);

    httpRequest.Query += `DocumentNo=${request.documentNo}`;
   
    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
     

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
