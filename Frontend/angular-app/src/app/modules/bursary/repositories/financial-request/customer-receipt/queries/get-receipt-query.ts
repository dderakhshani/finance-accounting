import {Inject} from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
import { NotificationService } from "src/app/shared/services/notification/notification.service";


export class GetReceiptQuery extends IRequest<GetReceiptQuery, FinancialRequest> {



  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: FinancialRequest): GetReceiptQuery {
    throw new ApplicationError(GetReceiptQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): FinancialRequest {
    throw new ApplicationError(GetReceiptQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/FinancialRequest/GetById";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetReceiptQueryHandler.name)
export class GetReceiptQueryHandler implements IRequestHandler<GetReceiptQuery, FinancialRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetReceiptQuery): Promise<FinancialRequest> {
    let httpRequest: HttpRequest<GetReceiptQuery> = new HttpRequest<GetReceiptQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<FinancialRequest>(httpRequest).toPromise().then(response => {
      return response
    })
  }
}
