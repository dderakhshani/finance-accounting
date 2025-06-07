import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { WarehouseRequestExitView } from "../../entities/warehouseRequestExit";
import { PaginatedList } from "../../../../core/models/paginated-list";


export class GetWarehouseRequestExitViewQuery extends IRequest<GetWarehouseRequestExitViewQuery, PaginatedList<WarehouseRequestExitView>> {


  constructor(

    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = ''
  ) {
    super();
  }

  mapFrom(entity: PaginatedList<WarehouseRequestExitView>): GetWarehouseRequestExitViewQuery {
    throw new ApplicationError(GetWarehouseRequestExitViewQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<WarehouseRequestExitView> {
    throw new ApplicationError(GetWarehouseRequestExitViewQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetWarehouseRequestExitView";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetWarehouseRequestExitViewQueryHandler.name)
export class GetWarehouseRequestExitViewQueryHandler implements IRequestHandler<GetWarehouseRequestExitViewQuery, PaginatedList<WarehouseRequestExitView>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetWarehouseRequestExitViewQuery): Promise<PaginatedList<WarehouseRequestExitView>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetWarehouseRequestExitViewQuery> = new HttpRequest<GetWarehouseRequestExitViewQuery>(request.url, request);

   
    
    if (request.fromDate != undefined) {
      httpRequest.Query += `&fromDate=${request.fromDate?.toUTCString()}`;
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `&toDate=${request.toDate?.toUTCString()}`;
    }
    
    
    return await this._httpService.Post<GetWarehouseRequestExitViewQuery, ServiceResult<PaginatedList<WarehouseRequestExitView>>>(httpRequest).toPromise().then(response => {
    
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
