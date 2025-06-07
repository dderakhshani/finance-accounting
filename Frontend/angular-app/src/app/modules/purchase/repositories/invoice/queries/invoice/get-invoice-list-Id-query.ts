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


export class GetInvoiceListIdQuery extends IRequest<GetInvoiceListIdQuery, invoice> {
  public ListId: string[] = [];
  constructor() {
    super();
  }


  mapFrom(entity: invoice): GetInvoiceListIdQuery {
    throw new ApplicationError(GetInvoiceListIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): invoice {
    throw new ApplicationError(GetInvoiceListIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/invoice/GetByListId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetInvoiceListIdQueryHandler.name)
export class GetInvoiceListIdQueryHandler implements IRequestHandler<GetInvoiceListIdQuery, GetInvoiceListIdQuery> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetInvoiceListIdQuery): Promise<GetInvoiceListIdQuery> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetInvoiceListIdQuery> = new HttpRequest<GetInvoiceListIdQuery>(request.url, request);


    return await this._httpService.Post<GetInvoiceListIdQuery, ServiceResult<GetInvoiceListIdQuery>>(httpRequest).toPromise().then(response => {

      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
