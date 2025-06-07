import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {MeasureUnitConversion} from "../../../entities/measure-unit-conversion";

export class GetMeasureUnitConversionsQuery extends IRequest<GetMeasureUnitConversionsQuery, MeasureUnitConversion[]> {

  constructor(public measureUnitId:number,public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: MeasureUnitConversion[]): GetMeasureUnitConversionsQuery {
    throw new ApplicationError(GetMeasureUnitConversionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MeasureUnitConversion[] {
    throw new ApplicationError(GetMeasureUnitConversionsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/measureUnitConversion/GetAllByMeasureUnitId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMeasureUnitConversionsQueryHandler.name)
export class GetMeasureUnitConversionsQueryHandler implements IRequestHandler<GetMeasureUnitConversionsQuery, MeasureUnitConversion[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetMeasureUnitConversionsQuery): Promise<MeasureUnitConversion[]> {
    let httpRequest: HttpRequest<GetMeasureUnitConversionsQuery> = new HttpRequest<GetMeasureUnitConversionsQuery>(request.url, request);
    httpRequest.Query  += `measureUnitId=${request.measureUnitId}`;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetMeasureUnitConversionsQuery, PaginatedList<MeasureUnitConversion>>(httpRequest).toPromise().then(response => {
      return response.data
    })
  }
}
