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

export class GetCostImportReceiptsQuery extends IRequest<GetCostImportReceiptsQuery, Number> {
  constructor(public referenceId: number) {
    super();
  }


  mapFrom(entity: Number): GetCostImportReceiptsQuery {
    throw new ApplicationError(GetCostImportReceiptsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Number {
    throw new ApplicationError(GetCostImportReceiptsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetCostImportReceipts";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCostImportReceiptsQueryHandler.name)
export class GetCostImportReceiptsQueryHandler implements IRequestHandler<GetCostImportReceiptsQuery, Number> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCostImportReceiptsQuery): Promise<Number> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCostImportReceiptsQuery> = new HttpRequest<GetCostImportReceiptsQuery>(request.url, request);
    httpRequest.Query += `referenceId=${request.referenceId}`;


    return await this._httpService.Get<ServiceResult<Number>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
