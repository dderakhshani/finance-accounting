import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { Receipt } from "../../../../entities/receipt";



export class getPurchaseRequestByIdQuery extends IRequest<getPurchaseRequestByIdQuery, Receipt> {
  constructor(public entityId: number) {
    
    super();
  }


  mapFrom(entity: Receipt): getPurchaseRequestByIdQuery {
    throw new ApplicationError(getPurchaseRequestByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(getPurchaseRequestByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/AraniRequest/GetPurchaseRequestById";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(getPurchaseRequestByIdQueryHandler.name)
export class getPurchaseRequestByIdQueryHandler implements IRequestHandler<getPurchaseRequestByIdQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
   
  )
  {
    
  }

  async Handle(request: getPurchaseRequestByIdQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<getPurchaseRequestByIdQuery> = new HttpRequest<getPurchaseRequestByIdQuery>(request.url, request);
    httpRequest.Query  += `requestId=${request.entityId}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

     
      return response.objResult

    }).finally(() => {
      this._notificationService.isLoader = false;
    })
    
  }
}
