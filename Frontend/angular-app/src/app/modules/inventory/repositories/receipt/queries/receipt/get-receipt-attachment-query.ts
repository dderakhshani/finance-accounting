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

export class GetRecepitAttachmentsQuery extends IRequest<GetRecepitAttachmentsQuery, number[]> {
 
  constructor(public documentHeadId:number) {
    super();
  }


  mapFrom(entity: number[]): GetRecepitAttachmentsQuery {
    throw new ApplicationError(GetRecepitAttachmentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): number[] {
    throw new ApplicationError(GetRecepitAttachmentsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetDocumentAttachment";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetRecepitAttachmentsQueryHandler.name)
export class GetRecepitAttachmentsQueryHandler implements IRequestHandler<GetRecepitAttachmentsQuery, number[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetRecepitAttachmentsQuery): Promise<number[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetRecepitAttachmentsQuery> = new HttpRequest<GetRecepitAttachmentsQuery>(request.url, request);
    httpRequest.Query += `DocumentHeadId=${request.documentHeadId}`;

    return await this._httpService.Get<ServiceResult<number[]>>(httpRequest).toPromise().then(response => {

      this._notificationService.isLoader = false;
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
