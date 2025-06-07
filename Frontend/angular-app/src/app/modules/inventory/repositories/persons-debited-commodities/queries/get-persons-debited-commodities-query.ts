import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { Receipt } from "../../../entities/receipt";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { PagesCommonService } from "../../../../../shared/services/pages/pages-common.service";
import { PersonsDebitedCommodities } from '../../../entities/persons-debited-commodities';



export class GetPersonsDebitedCommoditiesQuery extends IRequest<GetPersonsDebitedCommoditiesQuery, PaginatedList<PersonsDebitedCommodities>> {



  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }


  mapTo(): PaginatedList<PersonsDebitedCommodities> {
    throw new Error('Method not implemented.');
  }
  mapFrom(entity: PaginatedList<PersonsDebitedCommodities>): GetPersonsDebitedCommoditiesQuery {
    throw new Error('Method not implemented.');
  }
  get url(): string {
    return "/inventory/PersonsDebitedCommodities/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetPersonsDebitedCommoditiesQueryHandler.name)
export class GetPersonsDebitedCommoditiesQueryHandler implements IRequestHandler<GetPersonsDebitedCommoditiesQuery, PaginatedList<PersonsDebitedCommodities>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetPersonsDebitedCommoditiesQuery): Promise<PaginatedList<PersonsDebitedCommodities>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetPersonsDebitedCommoditiesQuery> = new HttpRequest<GetPersonsDebitedCommoditiesQuery>(request.url, request);


    httpRequest.Query += `fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;

    return await this._httpService.Post<GetPersonsDebitedCommoditiesQuery, ServiceResult<PaginatedList<PersonsDebitedCommodities>>>(httpRequest).toPromise().then(response => {
      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
