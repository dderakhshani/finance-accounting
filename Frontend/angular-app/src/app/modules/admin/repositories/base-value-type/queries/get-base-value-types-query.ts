import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {BaseValueType} from "../../../entities/base-value-type";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetBaseValueTypesQuery extends IRequest<GetBaseValueTypesQuery, PaginatedList<BaseValueType>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<BaseValueType>): GetBaseValueTypesQuery {
    throw new ApplicationError(GetBaseValueTypesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<BaseValueType> {
    throw new ApplicationError(GetBaseValueTypesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/BaseValueType/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBaseValueTypesQueryHandler.name)
export class GetBaseValueTypesQueryHandler implements IRequestHandler<GetBaseValueTypesQuery, PaginatedList<BaseValueType>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBaseValueTypesQuery): Promise<PaginatedList<BaseValueType>> {
    let httpRequest: HttpRequest<GetBaseValueTypesQuery> = new HttpRequest<GetBaseValueTypesQuery>(request.url, request);


    return await this._httpService.Post<GetBaseValueTypesQuery, ServiceResult<PaginatedList<BaseValueType>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
