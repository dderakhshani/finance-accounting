
import {Inject} from "@angular/core";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "../../../../../core/models/service-result";
import { Assets } from "../../../entities/Assets";


export class GetLastNumberQuery extends IRequest<Assets, string> {


  constructor(public assetGroupId: number
  

  ) {
    super();
  }

  mapFrom(entity: string): Assets {
    throw new Error("Method not implemented.");
  }

  mapTo(): string {
    throw new ApplicationError(GetLastNumberQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Assets/GetLastNumber";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetLastNumberQueryHandler.name)
export class GetLastNumberQueryHandler implements IRequestHandler<GetLastNumberQuery, string> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
   

  ) {
  }

  async Handle(request: GetLastNumberQuery): Promise<string> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetLastNumberQuery> = new HttpRequest<GetLastNumberQuery>(request.url, request);


    httpRequest.Query += `assetGroupId=${request.assetGroupId}`;

    return await this._httpService.Get<ServiceResult<string>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
