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



export class UpdateProductProperty extends IRequest<UpdateProductProperty, Receipt> {
  constructor(public Date: string) {
    
    super();
  }


  mapFrom(entity: Receipt): UpdateProductProperty {
    throw new ApplicationError(UpdateProductProperty.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(UpdateProductProperty.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/SinaRequest/UpdateProductProperty";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateProductPropertyHandler.name)
export class UpdateProductPropertyHandler implements IRequestHandler<UpdateProductProperty, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
   
  )
  {
    
  }

  async Handle(request: UpdateProductProperty): Promise<Receipt> {
   
    let httpRequest: HttpRequest<UpdateProductProperty> = new HttpRequest<UpdateProductProperty>(request.url, request);
    httpRequest.Query += `date=${request.Date}`;
    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
      return response.objResult

    })
    
  }
}
