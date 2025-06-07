import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {MoadianInvoiceHeader} from "../../../../entities/moadian-invoice-header";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";

export class GetMoadianInvoiceHeaderQuery extends IRequest<GetMoadianInvoiceHeaderQuery, MoadianInvoiceHeader> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: MoadianInvoiceHeader): GetMoadianInvoiceHeaderQuery {
    throw new ApplicationError(GetMoadianInvoiceHeaderQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MoadianInvoiceHeader {
    throw new ApplicationError(GetMoadianInvoiceHeaderQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/moadian/getInvoice";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMoadianInvoiceHeaderQueryHandler.name)
export class GetMoadianInvoiceHeaderQueryHandler implements IRequestHandler<GetMoadianInvoiceHeaderQuery, MoadianInvoiceHeader> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: GetMoadianInvoiceHeaderQuery): Promise<MoadianInvoiceHeader> {
    let httpRequest: HttpRequest<GetMoadianInvoiceHeaderQuery> = new HttpRequest<GetMoadianInvoiceHeaderQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Get<ServiceResult<MoadianInvoiceHeader>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
