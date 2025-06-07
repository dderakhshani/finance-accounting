import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {WarehouseLayout} from "../../../entities/warehouse-layout";

export class GetWarehouseLayoutQuery extends IRequest<GetWarehouseLayoutQuery, WarehouseLayout> {


   constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: WarehouseLayout): GetWarehouseLayoutQuery {
    throw new ApplicationError(GetWarehouseLayoutQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): WarehouseLayout {
    throw new ApplicationError(GetWarehouseLayoutQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/warehouselayout/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseLayoutQueryHandler.name)
export class GetWarehouseLayoutQueryHandler implements IRequestHandler<GetWarehouseLayoutQuery, WarehouseLayout> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseLayoutQuery): Promise<WarehouseLayout> {
    this._notificationService.isLoader = true;

    let httpRequest: HttpRequest<GetWarehouseLayoutQuery> = new HttpRequest<GetWarehouseLayoutQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Get<ServiceResult<WarehouseLayout>>(httpRequest).toPromise().then(response => {
     

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
