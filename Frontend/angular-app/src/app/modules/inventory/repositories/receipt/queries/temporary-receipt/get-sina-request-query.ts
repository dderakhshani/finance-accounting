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



export class GetProductInputToWarehouse extends IRequest<GetProductInputToWarehouse, Receipt> {
  constructor(public Date: string) {
    
    super();
  }


  mapFrom(entity: Receipt): GetProductInputToWarehouse {
    throw new ApplicationError(GetProductInputToWarehouse.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetProductInputToWarehouse.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/SinaRequest/GetProductInputToWarehouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetProductInputToWarehouseHandler.name)
export class GetProductInputToWarehouseHandler implements IRequestHandler<GetProductInputToWarehouse, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
   
  )
  {
    
  }

  async Handle(request: GetProductInputToWarehouse): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetProductInputToWarehouse> = new HttpRequest<GetProductInputToWarehouse>(request.url, request);
    httpRequest.Query += `date=${request.Date}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

     
      return response.objResult

    }).finally(() => {
      this._notificationService.isLoader = false;
    })
    
  }
}
