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

export class GetBomsQuery extends IRequest<GetBomsQuery, PaginatedList<Bom>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<Bom>): GetBomsQuery {
    throw new ApplicationError(GetBomsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<Bom> {
    throw new ApplicationError(GetBomsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bom/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBomsQueryHandler.name)
export class GetBomsQueryHandler implements IRequestHandler<GetBomsQuery, PaginatedList<Bom>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBomsQuery): Promise<PaginatedList<Bom>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetBomsQuery> = new HttpRequest<GetBomsQuery>(request.url, request);
    
    return await this._httpService.Post<GetBomsQuery, ServiceResult<PaginatedList<Bom>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
