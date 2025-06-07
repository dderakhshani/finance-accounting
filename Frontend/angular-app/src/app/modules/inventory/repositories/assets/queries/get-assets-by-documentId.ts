
import {Inject} from "@angular/core";

import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";

import { Assets } from "../../../entities/Assets";



export class GetAssetsByDocumentIdQuery extends IRequest<GetAssetsByDocumentIdQuery, Assets> {

  constructor(public documentHeadId: number,
    public commodityId: number, 
   
   ) {
    super();
  }

  mapFrom(entity: Assets): GetAssetsByDocumentIdQuery {
    throw new ApplicationError(GetAssetsByDocumentIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Assets {
    throw new ApplicationError(GetAssetsByDocumentIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Assets/GetByDocumentId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler( GetAssetsByDocumentIdQueryHandler.name)
export class  GetAssetsByDocumentIdQueryHandler implements IRequestHandler<GetAssetsByDocumentIdQuery, Assets> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAssetsByDocumentIdQuery): Promise<Assets> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetAssetsByDocumentIdQuery> = new HttpRequest<GetAssetsByDocumentIdQuery>(request.url, request);
    httpRequest.Query += `documentHeadId=${request.documentHeadId}&commodityId=${request.commodityId}`;

    return await this._httpService.Get<Assets>(httpRequest).toPromise().then(response => {
      return response
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
