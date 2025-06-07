
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import {  AssetsSerial } from "../../../entities/Assets";



export class GetDuplicateAssetsQuery extends IRequest<GetDuplicateAssetsQuery, AssetsSerial> {

  public AssetsSerial: AssetsSerial[] = [];
  constructor(
   
   ) {
    super();
  }

  mapFrom(entity: AssetsSerial): GetDuplicateAssetsQuery {
    throw new ApplicationError(GetDuplicateAssetsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AssetsSerial {
    throw new ApplicationError(GetDuplicateAssetsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Assets/GetDuplicateAssets";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler( GetDuplicateAssetsQueryHandler.name)
export class GetDuplicateAssetsQueryHandler implements IRequestHandler<GetDuplicateAssetsQuery, AssetsSerial> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDuplicateAssetsQuery): Promise<AssetsSerial> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetDuplicateAssetsQuery> = new HttpRequest<GetDuplicateAssetsQuery>(request.url, request);
    
    return await this._httpService.Post<GetDuplicateAssetsQuery, ServiceResult<AssetsSerial>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
