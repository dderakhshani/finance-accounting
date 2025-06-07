import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../core/models/paginated-list";
import { AccountReference } from "../../../accounting/entities/account-reference";
import { PagesCommonService } from "../../../../shared/services/pages/pages-common.service";



export class GetRequesterReferenceWarhouse extends IRequest<GetRequesterReferenceWarhouse, PaginatedList<AccountReference>> {

  constructor(
    public documentStauseBaseValue: number,
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public pageIndex: number = 0,
    public pageSize: number = 0,
    public conditions?: SearchQuery[],
    public orderByProperty: string = ''
  ) {
    super();
  }

  mapFrom(entity: PaginatedList<AccountReference>): GetRequesterReferenceWarhouse {
    throw new ApplicationError(GetRequesterReferenceWarhouse.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<AccountReference> {
    throw new ApplicationError(GetRequesterReferenceWarhouse.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/AccountReferences/GetRequesterReferenceWarhouse";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetRequesterReferenceWarhouseHandler.name)
export class GetRequesterReferenceWarhouseHandler implements IRequestHandler<GetRequesterReferenceWarhouse, PaginatedList<AccountReference>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService
  ) {
  }

  async Handle(request: GetRequesterReferenceWarhouse): Promise<PaginatedList<AccountReference>> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetRequesterReferenceWarhouse> = new HttpRequest<GetRequesterReferenceWarhouse>(request.url, request);


    httpRequest.Query += `documentStauseBaseValue=${request.documentStauseBaseValue}&fromDate=${request.fromDate?.toUTCString()}&toDate=${request.toDate?.toUTCString()}`;

    return await this._httpService.Post<GetRequesterReferenceWarhouse, ServiceResult<PaginatedList<AccountReference>>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
