import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { invoice } from "../../../../entities/invoice";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../../core/models/paginated-list";
import { Data } from "@angular/router";
import { PagesCommonService } from "../../../../../../shared/services/pages/pages-common.service";




export class GetInvoicesQuery extends IRequest<GetInvoicesQuery, PaginatedList<invoice>> {

  constructor(public codeVoucherGroupId: number,
    public isImportPurchase: boolean | null = null,
    public fromDate: Data | undefined = undefined,
    public toDate: Data | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = '') {
    super();
  }

  mapFrom(entity: PaginatedList<invoice>): GetInvoicesQuery {
    throw new ApplicationError(GetInvoicesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<invoice> {
    throw new ApplicationError(GetInvoicesQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/invoice/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetInvoicesQueryHandler.name)
export class GetInvoicesQueryHandler implements IRequestHandler<GetInvoicesQuery, PaginatedList<invoice>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: GetInvoicesQuery): Promise<PaginatedList<invoice>> {
    this._notificationService.isLoader = true;
    var _null = '';
    let httpRequest: HttpRequest<GetInvoicesQuery> = new HttpRequest<GetInvoicesQuery>(request.url, request);


    httpRequest.Query += `codeVoucherGroupId=${request.codeVoucherGroupId}&IsImportPurchase=${request.isImportPurchase != undefined ? request.isImportPurchase : _null}&fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;

    return await this._httpService.Post<GetInvoicesQuery, ServiceResult<PaginatedList<invoice>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
