import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {AutoVoucherFormula} from "../../../entities/AutoVoucherFormula";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetAutoVoucherFormulasQuery extends IRequest<GetAutoVoucherFormulasQuery, PaginatedList<AutoVoucherFormula>> {
  public voucherTypeId:string | undefined = undefined;
  public sourceVoucherTypeId:string | undefined = undefined;
  public voucherTypeTitle:string | undefined = undefined;
  public sourceVoucherTypeTitle:string | undefined = undefined;

  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<AutoVoucherFormula>): GetAutoVoucherFormulasQuery {
    throw new ApplicationError(GetAutoVoucherFormulasQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  mapTo(): PaginatedList<AutoVoucherFormula> {
    throw new ApplicationError(GetAutoVoucherFormulasQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/autoVoucherFormula/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAutoVoucherFormulasQueryHandler.name)
export class GetAutoVoucherFormulasQueryHandler implements IRequestHandler<GetAutoVoucherFormulasQuery, PaginatedList<AutoVoucherFormula>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAutoVoucherFormulasQuery): Promise<PaginatedList<AutoVoucherFormula>> {
    let httpRequest: HttpRequest<GetAutoVoucherFormulasQuery> = new HttpRequest<GetAutoVoucherFormulasQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetAutoVoucherFormulasQuery, ServiceResult<PaginatedList<AutoVoucherFormula>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}

