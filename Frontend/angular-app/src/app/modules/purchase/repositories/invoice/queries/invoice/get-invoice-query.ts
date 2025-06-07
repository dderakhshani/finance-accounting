import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { invoice } from "../../../../entities/invoice";

export class GetInvoiceQuery extends IRequest<GetInvoiceQuery, invoice> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: invoice): GetInvoiceQuery {
    throw new ApplicationError(GetInvoiceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): invoice {
    throw new ApplicationError(GetInvoiceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/invoice/Get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetInvoiceQueryHandler.name)
export class GetInvoiceQueryHandler implements IRequestHandler<GetInvoiceQuery, invoice> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetInvoiceQuery): Promise<invoice> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetInvoiceQuery> = new HttpRequest<GetInvoiceQuery>(request.url, request);
    httpRequest.Query += `Id=${request.entityId}`;


    return await this._httpService.Get<ServiceResult<invoice>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
