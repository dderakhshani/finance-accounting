import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { Assets } from "../../../entities/Assets";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import { PagesCommonService } from "../../../../../shared/services/pages/pages-common.service";

export class GetAssetsesQuery extends IRequest<GetAssetsesQuery, PaginatedList<Assets>> {


  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<Assets>): GetAssetsesQuery {
    throw new ApplicationError(GetAssetsesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Assets> {
    throw new ApplicationError(GetAssetsesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Assets/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAssetsesQueryHandler.name)
export class GetAssetsesQueryHandler implements IRequestHandler<GetAssetsesQuery, PaginatedList<Assets>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService
  ) {
  }

  async Handle(request: GetAssetsesQuery): Promise<PaginatedList<Assets>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetAssetsesQuery> = new HttpRequest<GetAssetsesQuery>(request.url, request);

    if (request.fromDate != undefined) {
      httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;
    }
    

    return await this._httpService.Post<GetAssetsesQuery, ServiceResult<PaginatedList<Assets>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
