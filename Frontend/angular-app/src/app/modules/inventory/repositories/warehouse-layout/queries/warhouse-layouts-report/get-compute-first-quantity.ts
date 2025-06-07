import { Inject } from "@angular/core";
import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../../core/models/paginated-list";
import { WarehouseLayoutsCommodity } from "../../../../entities/warehouse-layouts-commodity";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";

export class GetComputeFirstQuantityQuery extends IRequest<GetComputeFirstQuantityQuery, number> {

  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: number): GetComputeFirstQuantityQuery {
    throw new ApplicationError(GetComputeFirstQuantityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): number {
    throw new ApplicationError(GetComputeFirstQuantityQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/WarehouseLayoutReport/ComputeFirstQuantity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetComputeFirstQuantityQueryHandler.name)
export class GetComputeFirstQuantityQueryHandler implements IRequestHandler<GetComputeFirstQuantityQuery, number> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetComputeFirstQuantityQuery): Promise<number> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetComputeFirstQuantityQuery> = new HttpRequest<GetComputeFirstQuantityQuery>(request.url, request);

    httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;
    return await this._httpService.Post<GetComputeFirstQuantityQuery, ServiceResult<number>>(httpRequest).toPromise().then(response => {
      this._notificationService.isLoader = false;
      return response.objResult
    })

  }
}
