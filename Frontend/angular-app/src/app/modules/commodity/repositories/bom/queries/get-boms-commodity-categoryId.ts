import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import Bom from "../../../entities/bom";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";

export class GetBomsByCommodityCategoryIdQuery extends IRequest<GetBomsByCommodityCategoryIdQuery, PaginatedList<Bom>> {

  constructor(public commodityCategoryId :number,public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Bom>): GetBomsByCommodityCategoryIdQuery {
    throw new ApplicationError(GetBomsByCommodityCategoryIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Bom> {
    throw new ApplicationError(GetBomsByCommodityCategoryIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bomValueHeader/GetBomsByCommodityCategoryId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBomsByCommodityCategoryIdQueryHandler.name)
export class GetBomsByCommodityCategoryIdQueryHandler implements IRequestHandler<GetBomsByCommodityCategoryIdQuery, PaginatedList<Bom>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBomsByCommodityCategoryIdQuery): Promise<PaginatedList<Bom>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetBomsByCommodityCategoryIdQuery> = new HttpRequest<GetBomsByCommodityCategoryIdQuery>(request.url, request);
    //httpRequest.Query += `commodityCategoryId=${request.commodityCategoryId}`;
    return await this._httpService.Post<GetBomsByCommodityCategoryIdQuery, ServiceResult<PaginatedList<Bom>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
