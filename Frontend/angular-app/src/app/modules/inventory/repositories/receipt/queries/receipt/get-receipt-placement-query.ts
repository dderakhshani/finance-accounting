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

export class GetRecepitPlacementQuery extends IRequest<GetRecepitPlacementQuery, Receipt> {
  constructor(public entityId: number, public warhouseId: number ) {
    super();
  }


  mapFrom(entity: Receipt): GetRecepitPlacementQuery {
    throw new ApplicationError(GetRecepitPlacementQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetRecepitPlacementQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetPlacementById";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetRecepitPlacementQueryHandler.name)
export class GetRecepitPlacementQueryHandler implements IRequestHandler<GetRecepitPlacementQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetRecepitPlacementQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetRecepitPlacementQuery> = new HttpRequest<GetRecepitPlacementQuery>(request.url, request);
    httpRequest.Query += `Id=${request.entityId}&warehouseId=${request.warhouseId}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
