import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PersonAddress} from "../../../entities/person-address";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetPersonAddressesQuery extends IRequest<GetPersonAddressesQuery, PaginatedList<PersonAddress>> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<PersonAddress>): GetPersonAddressesQuery {
    throw new ApplicationError(GetPersonAddressesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<PersonAddress> {
    throw new ApplicationError(GetPersonAddressesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personAddress/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetPersonAddressesQueryHandler.name)
export class GetPersonAddressesQueryHandler implements IRequestHandler<GetPersonAddressesQuery, PaginatedList<PersonAddress>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetPersonAddressesQuery): Promise<PaginatedList<PersonAddress>> {
    let httpRequest: HttpRequest<GetPersonAddressesQuery> = new HttpRequest<GetPersonAddressesQuery>(request.url, request);

    return await this._httpService.Post<GetPersonAddressesQuery, ServiceResult<PaginatedList<PersonAddress>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
