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
import { DocumentHeadExtraCost } from "../../../entities/documentHeadExtraCost";

export class GetTotalExtraCostQuery extends IRequest<GetTotalExtraCostQuery, Number> {

  public listIds: number[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: Number): GetTotalExtraCostQuery {
    throw new ApplicationError(GetTotalExtraCostQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Number {
    throw new ApplicationError(GetTotalExtraCostQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/DocumentHeadExtraCost/GetTotalExtraCost";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetTotalExtraCostQueryHandler.name)
export class GetTotalExtraCostQueryHandler implements IRequestHandler<GetTotalExtraCostQuery, Number> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetTotalExtraCostQuery): Promise<Number> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetTotalExtraCostQuery> = new HttpRequest<GetTotalExtraCostQuery>(request.url, request);


    return await this._httpService.Post<GetTotalExtraCostQuery, ServiceResult<Number>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
