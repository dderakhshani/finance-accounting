import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { TicketModel } from "../../../entities/ticket";


export class GetTicketListQuery extends IRequest<GetTicketListQuery, PaginatedList<TicketModel>> {


  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: PaginatedList<TicketModel>): GetTicketListQuery {
    throw new ApplicationError(GetTicketListQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<TicketModel> {
    throw new ApplicationError(GetTicketListQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/ticketing/Ticket/GetTicketList";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetTicketListQueryHandler.name)
export class GetTicketListQueryHandler implements IRequestHandler<GetTicketListQuery, PaginatedList<TicketModel>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService) {
  }

  async Handle(request: GetTicketListQuery): Promise<PaginatedList<TicketModel>> {
    let httpRequest: HttpRequest<GetTicketListQuery> = new HttpRequest<GetTicketListQuery>(request.url, request);
    return await this._httpService.Post<GetTicketListQuery, ServiceResult<PaginatedList<TicketModel>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })


  }
}
