import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { MakeProductPrice } from "../../entities/makeProductPrice";
import { PaginatedList } from "../../../../core/models/paginated-list";


export class GetMakeProductPriceQuery extends IRequest<GetMakeProductPriceQuery, PaginatedList<MakeProductPrice>> {


  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,

    public pageIndex: number = 0,
    public pageSize: number = 0,

    public conditions?: SearchQuery[]

  ) {
    super();
  }

  mapFrom(entity: PaginatedList<MakeProductPrice>): GetMakeProductPriceQuery {
    throw new ApplicationError(GetMakeProductPriceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<MakeProductPrice> {
    throw new ApplicationError(GetMakeProductPriceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetMakeProductPriceReport";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetMakeProductPriceQueryHandler.name)
export class GetMakeProductPriceQueryHandler implements IRequestHandler<GetMakeProductPriceQuery, PaginatedList<MakeProductPrice>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetMakeProductPriceQuery): Promise<PaginatedList<MakeProductPrice>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetMakeProductPriceQuery> = new HttpRequest<GetMakeProductPriceQuery>(request.url, request);



    if (request.fromDate != undefined) {
      httpRequest.Query += `&fromDate=${request.fromDate?.toUTCString()}`;
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `&toDate=${request.toDate?.toUTCString()}`;
    }


    return await this._httpService.Post<GetMakeProductPriceQuery, ServiceResult<PaginatedList<MakeProductPrice>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
