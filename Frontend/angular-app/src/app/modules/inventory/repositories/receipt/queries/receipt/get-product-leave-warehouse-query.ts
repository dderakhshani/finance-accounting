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



export class GetProductLeaveWarehouse extends IRequest<GetProductLeaveWarehouse, Receipt> {
  constructor(public Date: string) {
    
    super();
  }


  mapFrom(entity: Receipt): GetProductLeaveWarehouse {
    throw new ApplicationError(GetProductLeaveWarehouse.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetProductLeaveWarehouse.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/SinaRequest/GetProductLeaveWarehouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetProductLeaveWarehouseHandler.name)
export class GetProductLeaveWarehouseHandler implements IRequestHandler<GetProductLeaveWarehouse, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
   
  )
  {
    
  }

  async Handle(request: GetProductLeaveWarehouse): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetProductLeaveWarehouse> = new HttpRequest<GetProductLeaveWarehouse>(request.url, request);
    httpRequest.Query += `date=${request.Date}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

     
      return response.objResult

    }).finally(() => {
      this._notificationService.isLoader = false;
    })
    
  }
}
