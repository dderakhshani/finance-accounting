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

export class GetRecepitGetByDocumentIdQuery extends IRequest<GetRecepitGetByDocumentIdQuery, Receipt> {
  constructor(public DocumentId: number) {
    super();
  }


  mapFrom(entity: Receipt): GetRecepitGetByDocumentIdQuery {
    throw new ApplicationError(GetRecepitGetByDocumentIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetRecepitGetByDocumentIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetByDocumentId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetRecepitGetByDocumentIdQueryHandler.name)
export class GetRecepitGetByDocumentIdQueryHandler implements IRequestHandler<GetRecepitGetByDocumentIdQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetRecepitGetByDocumentIdQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetRecepitGetByDocumentIdQuery> = new HttpRequest<GetRecepitGetByDocumentIdQuery>(request.url, request);
    httpRequest.Query += `DocumentId=${request.DocumentId}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
