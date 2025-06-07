import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {MeasureUnit} from "../../../entities/measure-unit";

export class GetMeasureUnitsQuery extends IRequest<GetMeasureUnitsQuery, MeasureUnit[]> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: MeasureUnit[]): GetMeasureUnitsQuery {
    throw new ApplicationError(GetMeasureUnitsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MeasureUnit[] {
    throw new ApplicationError(GetMeasureUnitsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/measureUnit/getAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMeasureUnitsQueryHandler.name)
export class GetMeasureUnitsQueryHandler implements IRequestHandler<GetMeasureUnitsQuery, MeasureUnit[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetMeasureUnitsQuery): Promise<MeasureUnit[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetMeasureUnitsQuery> = new HttpRequest<GetMeasureUnitsQuery>(request.url, request);
    return await this._httpService.Post<GetMeasureUnitsQuery, PaginatedList<MeasureUnit>>(httpRequest).toPromise().then(response => {
      return response.data
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
