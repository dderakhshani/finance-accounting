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

export class GetByDocumentNoAndDocumentStauseBaseValueQuery extends IRequest<GetByDocumentNoAndDocumentStauseBaseValueQuery, Receipt> {
  constructor(public DocumentNo: number, public documentStauseBaseValue: number ) {
    super();
  }


  mapFrom(entity: Receipt): GetByDocumentNoAndDocumentStauseBaseValueQuery {
    throw new ApplicationError(GetByDocumentNoAndDocumentStauseBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetByDocumentNoAndDocumentStauseBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetByDocumentNoAndDocumentStauseBaseValue";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetByDocumentNoAndDocumentStauseBaseValueQueryHandler.name)
export class GetByDocumentNoAndDocumentStauseBaseValueQueryHandler implements IRequestHandler<GetByDocumentNoAndDocumentStauseBaseValueQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetByDocumentNoAndDocumentStauseBaseValueQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetByDocumentNoAndDocumentStauseBaseValueQuery> = new HttpRequest<GetByDocumentNoAndDocumentStauseBaseValueQuery>(request.url, request);
    httpRequest.Query += `DocumentNo=${request.DocumentNo}&documentStauseBaseValue=${request.documentStauseBaseValue}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
