import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {BaseValue} from "../../../entities/base-value";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";

export class GetBaseValuesQuery extends IRequest<GetBaseValuesQuery, BaseValue[]> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: BaseValue[]): GetBaseValuesQuery {
    throw new ApplicationError(GetBaseValuesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BaseValue[] {
    throw new ApplicationError(GetBaseValuesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/BaseValue/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBaseValuesQueryHandler.name)
export class GetBaseValuesQueryHandler implements IRequestHandler<GetBaseValuesQuery, BaseValue[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBaseValuesQuery): Promise<BaseValue[]> {
    let httpRequest: HttpRequest<GetBaseValuesQuery> = new HttpRequest<GetBaseValuesQuery>(request.url, request);


    return await this._httpService.Post<GetBaseValuesQuery, ServiceResult<PaginatedList<BaseValue>>>(httpRequest).toPromise().then(response => {
      return response.objResult.data
    })


  }
}
